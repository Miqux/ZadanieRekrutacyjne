using CsvHelper.Configuration;
using ZadanieRekrutacyjne.DAL.Models;

namespace ZadanieRekrutacyjne.Services
{
    public interface IDownloadFileService
    {
        Task<ResultModel<string>> SaveCsvAsync(string fileName, string localPath);
        List<T> ReadCsv<T, Y>(string filePath, string delimiter, bool headerRecord = true) where Y : ClassMap;
    }
}
