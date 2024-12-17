namespace Shared.SeedWorks
{
    public class ApiResult<T>
    {
        public ApiResult()
        {
        }

        public ApiResult(bool isSucceeded, string message = "")
        {
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public ApiResult(bool isSucceeded, T data, string message = "")
        {
            Data = data;
            Message = message;
            IsSucceeded = isSucceeded;
        }

        public bool IsSucceeded { get; set; }
        public string Message { get; set; }
        public T Data { get; }
    }
}
