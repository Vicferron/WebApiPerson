using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Test.WebPerson.Service
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly object _content;

        public MockHttpMessageHandler(HttpStatusCode statusCode, object content = null)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode);

            if (_content != null)
            {
                response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(_content));
            }

            return await Task.FromResult(response);
        }
    }
}
