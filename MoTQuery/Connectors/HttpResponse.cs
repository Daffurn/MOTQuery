using MOTQuery.Interface;

namespace MOTQuery.Connectors
{
    internal class HttpResponse : IResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public string Body { get; set; }

        public HttpResponse()
        {
            ErrorMessage = "";
            Body = "";
        }
    }
};


