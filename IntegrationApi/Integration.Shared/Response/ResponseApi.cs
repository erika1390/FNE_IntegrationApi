namespace Integration.Shared.Response
{
    public class ResponseApi<T>
    {
        public ResponseApi(T data,
                            bool state,
                            string? message)
        {
            Data = data;
            State = state;
            Message = message;
        }
        public bool State { get; set; }
        public string? Message { get; set; }
        public T? Data { get; }
    }
}