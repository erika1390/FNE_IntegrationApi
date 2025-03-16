namespace Integration.Shared.Response
{
    public class ResponseApi<T>
    {
        public T? Data { get; set; }
        public bool State { get; set; }
        public string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public ResponseApi(T? data, bool state, string message, IEnumerable<string>? errors = null)
        {
            Data = data;
            State = state;
            Message = message;
            Errors = errors;
        }

        public static ResponseApi<T> Success(T data, string message = "Operación exitosa")
        {
            return new ResponseApi<T>(data, true, message);
        }

        public static ResponseApi<T> Error(string message)
        {
            return new ResponseApi<T>(default, false, message, new List<string> { message });
        }

        public static ResponseApi<T> Error(IEnumerable<string> errors, string message = "Errores en la validación.")
        {
            return new ResponseApi<T>(default, false, message, errors);
        }
    }
}