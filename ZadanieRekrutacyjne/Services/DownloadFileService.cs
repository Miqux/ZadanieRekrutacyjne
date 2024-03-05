using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using ZadanieRekrutacyjne.Models;

namespace ZadanieRekrutacyjne.Services
{
    public class DownloadFileService : IDownloadFileService
    {
        public async Task<ResultModel<List<T>>> DownloadAndSaveCsvFile<T, Y>(string fileName, string localPath, string delimiter, bool headerRecord = true)
            where Y : ClassMap
        {
            string blobUrl = "https://rekturacjazadanie.blob.core.windows.net/zadanie/" + fileName;
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(blobUrl);
            List<T> products = new();

            if (response.IsSuccessStatusCode)
            {
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

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, configuration))
                {
                    csv.Context.RegisterClassMap<Y>();
                    products = csv.GetRecords<T>().ToList();

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                    streamWrite = new FileStream(localPath + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await contentStream.CopyToAsync(streamWrite);
                    }
                }

                return new ResultModel<List<T>>(true, products);
            }
            else
            {
                return new ResultModel<List<T>>(false, $"Wystąpił błąd: {response.StatusCode}");
            }
        }
    }
}
