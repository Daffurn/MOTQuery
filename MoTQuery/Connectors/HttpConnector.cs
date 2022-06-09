using MOTQuery.Interface;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MOTQueryTests")]
namespace MOTQuery.Connectors
{
    internal class HttpConnector : IConnector
    {
        private readonly string Endpoint;

        private static HttpClient Client = new();

        public HttpConnector(string endpoint, string apiKey)
        {
            Endpoint = endpoint;
            Client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        internal HttpConnector(string endpoint, string apiKey, HttpMessageHandler handler) : this(endpoint, apiKey)
        {
            Client = new HttpClient(handler);
            Client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        public IResponse Send(IInput input)
        {
            HttpResponse response = new();
             
            try
            {
                var endpoint = Endpoint + $"?registration={input.RegistrationNumber}";
                Task<string> t = Client.GetStringAsync(endpoint);
                t.Wait();
                response.Success = true;
                response.Body = t.Result;

                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorMessage = e.Message;
            }

            return response;
        }
    }
}
