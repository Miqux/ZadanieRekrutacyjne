using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        public async Task<ResultModel<string>> SaveCsvAsync(string fileName, string localPath)
        {
            string blobUrl = "https://rekturacjazadanie.blob.core.windows.net/zadanie/" + fileName;
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(blobUrl);

            if (response.IsSuccessStatusCode)
            {
                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                    streamWriter = new FileStream(localPath + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await contentStream.CopyToAsync(streamWriter);
                }
                return new ResultModel<string>(true, value: localPath + fileName);
            }
            else
            {
                return new ResultModel<string>(false, $"Wystąpił błąd: {response.StatusCode}");
            }
        }
        public List<T> ReadCsv<T, Y>(string filePath, string delimiter, bool headerRecord = true) where Y : ClassMap
        {
            List<T> products = new();

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.None,
                Delimiter = delimiter,
                HasHeaderRecord = headerRecord,
                MissingFieldFound = null,
                IgnoreBlankLines = true,
                ShouldSkipRecord = record => { return record.Row[0] == "__empty_line__"; },
                BadDataFound = null,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap<Y>();
                products = csv.GetRecords<T>().ToList();
            }

            return products;
        }
    }
}
