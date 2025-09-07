using api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;

namespace api.Services
{
    public interface ICSVService
    {
        bool UploadCSV(UploadedBase64 csvBase4);
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
            "Issue id",
            "Issue key"

        ];
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public CsvReader GetCsvReader(string base64Content)
        {
            var commaIndex = base64Content.IndexOf(',');
            if (commaIndex >= 0)
                base64Content = base64Content[(commaIndex + 1)..];

            var bytes = Convert.FromBase64String(base64Content);

            var memoryStream = new MemoryStream(bytes);
            var reader = new StreamReader(memoryStream);
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                BadDataFound = null
            });

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

        public string GetJsonDataFromCSV(CsvReader csv)
        {
            var records = new List<Dictionary<string, object>>();
            
            if (!csv.Read()) return "[]";
            csv.ReadHeader();

            var headers = csv.HeaderRecord;

            if (headers == null) return "[]";

            var relevantHeaders = headers
                .Where(h => !irrelevantColumns.Contains(h, StringComparer.OrdinalIgnoreCase))
                .ToList();

            while (csv.Read())
            {
                var row = new Dictionary<string, object>();

                foreach (var header in relevantHeaders)
                {
                    var value = csv.GetField(header);
                    row[header] = value ?? string.Empty;
                }

                records.Add(row);
            }

            return JsonSerializer.Serialize(records, JsonOptions);
        }


        public bool UploadCSV(UploadedBase64 csvBase64)
        {
            var csv = GetCsvReader(csvBase64.Base64Content!);
            if (!ValidateCSV(csv)) return false;

            csv = GetCsvReader(csvBase64.Base64Content!);
            string dataFromCsv = GetJsonDataFromCSV(csv);

            Console.WriteLine(dataFromCsv);
            return true;
        }

    }
}