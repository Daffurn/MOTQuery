using Moq.Protected;
using System.Net;

namespace MOTQueryTests
{
    internal class TestUtils
    {
        public static Mock<HttpMessageHandler> GetMockHttpMessageHandler(string content = "Success")
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            //Mock SendAsync which is used by GetStringAsync
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    string regNumber = request.RequestUri.Query.Replace("?registration=", "");

                    HttpResponseMessage message = new();

                    if (string.Equals(regNumber, "TEST123"))
                    {
                        message.StatusCode = HttpStatusCode.OK;
                        message.Content = new StringContent(content);
                    }
                    else
                    {
                        message.StatusCode = HttpStatusCode.NotFound;
                    }

                    return message;
                }
                )
                .Verifiable();

            return mockMessageHandler;
        }
    }
}
