using api.Mappings;
using api.Models;
using api.Repository;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace api.Services
{
    public interface ICSVService
    {
        List<IssueFromCSV>? GetIssuesFromCSV(UploadedBase64 csvBase64);
        bool InsertData(UploadedBase64 csvBase64);
    }

    public class CSVService : ICSVService
    {
        // Columns to ignore when mapping CSV to IssueFromCSV
        private static readonly HashSet<string> IrrelevantColumns = new(StringComparer.OrdinalIgnoreCase)
        {
            "StatusCategoryChanged", "StatusCategory", "Parent", "Comment",
            "CustomfieldVulnerability", "CustomfieldTeam", "CustomfieldRank",
            "CustomfieldIssuecolor", "CustomfieldDevelopment", "OutwardissuelinkBlocks",
            "InwardissuelinkBlocks", "SecurityLevel", "ΣTimeSpent", "ΣRemainingEstimate",
            "ΣOriginalEstimate", "WorkRatio", "TimeSpent", "RemainingEstimate",
            "Originalestimate", "Watchers", "WatchersId", "Environment", "LastViewed",
            "Updated", "Projectleadid", "Projecttype", "Projectkey", "Issuekey"
        };

        // CSV configuration with custom header normalization
        private static readonly CsvConfiguration Config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => NormalizeHeader(args.Header),
            MissingFieldFound = null,
            HeaderValidated = null
        };

        // Clean data
        private static string NormalizeHeader(string header)
        {
            return header.Replace(" ", "")
                         .Replace("(", "")
                         .Replace(")", "")
                         .Replace("-", "")
                         .Replace("Î", "")
                         .Replace("£", "");
        }

        // CSV Reader
        private static CsvReader CreateCsvReader(string base64Content)
        {
            var commaIndex = base64Content.IndexOf(',');
            if (commaIndex >= 0) base64Content = base64Content[(commaIndex + 1)..];

            var bytes = Convert.FromBase64String(base64Content);
            var reader = new StreamReader(new MemoryStream(bytes), Encoding.UTF8);
            return new CsvReader(reader, Config);
        }

        // Validate CSV

        private static bool ValidateCSV(CsvReader csv)
        {
            if (!csv.Read()) return false;
            csv.ReadHeader();
            var headerLength = csv.HeaderRecord?.Length ?? 0;
            if (headerLength == 0) return false;

            while (csv.Read())
            {
                if (csv.Parser.Count != headerLength)
                    return false;
            }
            return true;
        }

        // Get data from CSV
        private static List<IssueFromCSV> GetDataFromCSV(CsvReader csv)
        {
            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();

            // Keep only relevant headers
            var relevantHeaders = headers
                .Where(h => !IrrelevantColumns.Contains(NormalizeHeader(h)))
                .ToList();

            // Map CSV headers to IssueFromCSV properties
            var map = new DefaultClassMap<IssueFromCSV>();
            var properties = typeof(IssueFromCSV).GetProperties();

            foreach (var header in relevantHeaders)
            {
                var prop = properties.FirstOrDefault(p =>
                    string.Equals(p.Name, NormalizeHeader(header), StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    map.Map(typeof(IssueFromCSV), prop).Name(header);
                }
            }

            csv.Context.RegisterClassMap(map);

            // Return all records
            return [.. csv.GetRecords<IssueFromCSV>()];
        }


        public List<IssueFromCSV>? GetIssuesFromCSV(UploadedBase64 csvBase64)
        {
            try
            {
                var csv = CreateCsvReader(csvBase64.Base64Content!);

                if (!ValidateCSV(csv))
                    throw new Exception("415"); // Invalid CSV structure

                csv = CreateCsvReader(csvBase64.Base64Content!);
                return GetDataFromCSV(csv);
            }
            catch (BadDataException)
            {
                // Any CSV parsing error → treat as Unsupported Media Type
                throw new Exception("415");
            }
            catch (HeaderValidationException)
            {
                // Header mismatch → also Unsupported Media Type
                throw new Exception("415");
            }
        }

        // Insert data parsed from GetIssuesFromCSV into the database
        public bool InsertData(UploadedBase64 csvBase64)
        {
            // Parse CSV into IssueFromCSV objects
            List<IssueFromCSV>? issueFromCSVs = GetIssuesFromCSV(csvBase64);

            // Map CSV data to Project entity
            Project? project = ProjectMapper.ToProject(issueFromCSVs);

            // Insert project into the database
            List<Project>? projects = new ProjectRepository().PostProject(project!);

            // Initialize Issue template
            Issue issue = new();

            // Initialize ImportService for batch issue insertion
            ImportService importService = new(projects![0], issue);

            // Build and insert each issue from CSV
            issueFromCSVs!.ForEach(issueData =>
            {
                importService.BuildIssue(issueData);
            });

            return true;
        }
    }
}
