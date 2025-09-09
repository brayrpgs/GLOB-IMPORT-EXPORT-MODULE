using api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;

namespace api.Services
{
    public interface ICSVService
    {
        List<IssueFromCSV>? UploadCSV(UploadedBase64 csvBase64);
    }

    public class CSVService : ICSVService
    {

        public static readonly string[] irrelevantColumns = [
            "Status Category Changed",
            "Status Category",
            "Parent",
            "Comment",
            "Custom field (Vulnerability)",
            "Custom field (Team)",
            "Custom field (Rank)",
            "Custom field (Issue color)",
            "Custom field (Development)",
            "Outward issue link (Blocks)",
            "Inward issue link (Blocks)",
            "Security Level",
            "Î£ Time Spent",
            "Î£ Remaining Estimate",
            "Î£ Original Estimate",
            "Work Ratio",
            "Time Spent",
            "Remaining Estimate",
            "Original estimate",
            "Watchers",
            "Watchers Id",
            "Environment",
            "Last Viewed",
            "Updated",
            "Project lead id",
            "Project type",
            "Project key",
            "Issue key"

        ];

        public static readonly CsvConfiguration Config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header
                .Replace(" ", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace("Î", ""),
            MissingFieldFound = null,
            HeaderValidated = null,
        };


        public CsvReader GetCsvReader(string base64Content)
        {


            var commaIndex = base64Content.IndexOf(',');
            if (commaIndex >= 0)
                base64Content = base64Content[(commaIndex + 1)..];

            var bytes = Convert.FromBase64String(base64Content);

            var memoryStream = new MemoryStream(bytes);
            var reader = new StreamReader(memoryStream);
            var csv = new CsvReader(reader, Config);

            return csv;
        }

        public bool ValidateCSV(CsvReader csv)
        {
            if (!csv.Read()) return false;
            csv.ReadHeader();
            var header = csv.HeaderRecord;
            if (header == null || header.Length == 0) return false;

            while (csv.Read())
            {
                if (csv.Parser.Count != header.Length)
                    return false;
            }

            return true;
        }

        public List<IssueFromCSV> GetDataFromCVS(CsvReader csv)
        {

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            var relevantHeaders = headers!
                .Where(h =>
                {
                    
                    var normalized = h.Replace(" ", "")
                                    .Replace("(", "")
                                    .Replace(")", "")
                                    .Replace("-", "")
                                    .Replace("Î", "")
                                    .ToLower();

                    return !irrelevantColumns.Any(c =>
                        c.Replace(" ", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", "")
                        .Replace("Î", "")
                        .ToLower() == normalized);
                })
                .Select(h => h.Replace(" ", "")
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("-", "")
                            .Replace("Î", "")) 
                .ToList();

            
            var map = new DefaultClassMap<IssueFromCSV>();
            foreach (var header in relevantHeaders)
            {
                
                var prop = typeof(IssueFromCSV).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, header, StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    map.Map(typeof(IssueFromCSV), prop).Name(header);
                }
            }

           
            csv.Context.RegisterClassMap(map);

            
            List<IssueFromCSV> records = csv.GetRecords<IssueFromCSV>().ToList();

            return records;

        }

        public List<IssueFromCSV>? UploadCSV(UploadedBase64 csvBase64)
        {
            var csv = GetCsvReader(csvBase64.Base64Content!);
            if (!ValidateCSV(csv)) return null;

            csv = GetCsvReader(csvBase64.Base64Content!);

            return GetDataFromCVS(csv) ?? null;
        }

    }
}