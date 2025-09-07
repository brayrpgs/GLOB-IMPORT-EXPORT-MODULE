using api.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
namespace api.Services
{
    public interface ICSVService
    {
        bool UploadCSV(UploadedBase64 csvBase4);
    }

    public class CSVService : ICSVService
    {

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

        public bool UploadCSV(UploadedBase64 csvBase64)
        {
            var csv = GetCsvReader(csvBase64.Base64Content!);

            if (!ValidateCSV(csv)) return false;

            return true;
        }



    }
}