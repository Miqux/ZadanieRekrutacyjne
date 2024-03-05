namespace ZadanieRekrutacyjne.DAL.Models
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Value { get; set; }

        public ResultModel(bool success, T value)
        {
            Success = success;
            Value = value;
        }
        public ResultModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
