namespace Curd.Common.ViewModel
{
    public class ResponseViewModel<T>
    {
        public string Message { get; set; }

        public string TechnicalError { get; set; }

        public int StatusCode { get; set; }

        public bool Success { get; set; }

        public T Object { get; set; }
    }
}
