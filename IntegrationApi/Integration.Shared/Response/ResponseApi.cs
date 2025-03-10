namespace Integration.Shared.Response
{
    public class ResponseApi<T>
    {
        public ResponseApi(T? data, bool state, string? message)
        {
            Data = data;
            State = state;
            Message = message;
        }

        public bool State { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public static ResponseApi<T> Success(T data, string message = "Operación exitosa")
        {
            return new ResponseApi<T>(data, true, message);
        }

        public static ResponseApi<T> Error(string message)
        {
            return new ResponseApi<T>(default, false, message);
        }
    }
}