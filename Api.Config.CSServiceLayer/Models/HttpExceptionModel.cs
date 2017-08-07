namespace Api.Config.CSServiceLayer.Models
{
    public class HttpExceptionModel
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Error { get; set; }
        public HttpExceptionModel(string method, string path, string error)
        {
            Method = method;
            Path = path;
            Error = error;
        }
    }
}
