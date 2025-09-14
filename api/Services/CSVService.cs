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
        List<IssueFromCSV>? UploadCSV(UploadedBase64 csvBase64);
        public bool InsertData(UploadedBase64 csvBase64);
    }

    public class CSVService : ICSVService
    {
        private static readonly HashSet<string> IrrelevantColumns = new(StringComparer.OrdinalIgnoreCase)
        {
            "StatusCategoryChanged",
            "StatusCategory",
            "Parent",
            "Comment",
            "CustomfieldVulnerability",
            "CustomfieldTeam",
            "CustomfieldRank",
            "CustomfieldIssuecolor",
            "CustomfieldDevelopment",
            "OutwardissuelinkBlocks",
            "InwardissuelinkBlocks",
            "SecurityLevel",
            "ΣTimeSpent",
            "ΣRemainingEstimate",
            "ΣOriginalEstimate",
            "WorkRatio",
            "TimeSpent",
            "RemainingEstimate",
            "Originalestimate",
            "Watchers",
            "WatchersId",
            "Environment",
            "LastViewed",
            "Updated",
            "Projectleadid",
            "Projecttype",
            "Projectkey",
            "Issuekey"
        };

        private static readonly CsvConfiguration Config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => NormalizeHeader(args.Header),
            MissingFieldFound = null,
            HeaderValidated = null
        };

        private static string NormalizeHeader(string header)
        {
            return header.Replace(" ", "")
                         .Replace("(", "")
                         .Replace(")", "")
                         .Replace("-", "")
                         .Replace("Î", "")
                         .Replace("£", "");
        }

        private static CsvReader CreateCsvReader(string base64Content)
        {
            var commaIndex = base64Content.IndexOf(',');
            if (commaIndex >= 0) base64Content = base64Content[(commaIndex + 1)..];

            var bytes = Convert.FromBase64String(base64Content);
            var reader = new StreamReader(new MemoryStream(bytes), Encoding.UTF8);
            return new CsvReader(reader, Config);
        }

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

        private static List<IssueFromCSV> GetDataFromCSV(CsvReader csv)
        {
            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();

            var relevantHeaders = headers
                .Where(h => !IrrelevantColumns.Contains(NormalizeHeader(h)))
                .ToList();

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
            return [.. csv.GetRecords<IssueFromCSV>()];
        }

        public List<IssueFromCSV>? UploadCSV(UploadedBase64 csvBase64)
        {
            var csv = CreateCsvReader(csvBase64.Base64Content!);
            if (!ValidateCSV(csv)) throw new Exception("415");

            csv = CreateCsvReader(csvBase64.Base64Content!);
            return GetDataFromCSV(csv);
        }

        public bool InsertData(UploadedBase64 csvBase64)
        {
            // recover all data from csv
            List<IssueFromCSV>? issueFromCSVs = UploadCSV(csvBase64);

            // create the project from class mappers
            Project? project = ProjectMapper.ToProject(issueFromCSVs);

            // insert proyect to DB
            List<Project>? projects = new ProjectRepository().PostProject(project!);

            // create a new issue
            Issue issue = new();

            // initialice every issue for insert to database
            ImportService importService = new(projects![0], issue);

            issueFromCSVs!.ForEach(issue =>
            {
                importService.BuildIssue(issue);   
            });

            return true;
        }

    }
}
