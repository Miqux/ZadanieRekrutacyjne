using CsvHelper.Configuration;
using ZadanieRekrutacyjne.Models;

namespace ZadanieRekrutacyjne.Services
{
    public interface IDownloadFileService
    {
        public Task<ResultModel<List<T>>> DownloadAndSaveCsvFile<T, Y>(string fileName, string localPath, string delimiter, bool headerRecord = true) where Y : ClassMap;
    }
}
