//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;

namespace IDSClient.Shared.Connectors
{
    public class HttpRequestService
    {
        private static HttpClient httpClient;
        

        public HttpRequestService()        
        {

        }

        private static void InitHttpClient()
        {
            var unoHttpClientHandler = new UnoHttpClientHandler();
            unoHttpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            // HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; (
            // ClientCertificateOption.Manual;
            unoHttpClientHandler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, certChain, policyErrors) =>
                {
                    return true;
                };

            httpClient = new HttpClient(unoHttpClientHandler, false)
            {
                // BaseAddress = new Uri("https://192.168.1.226:5001/api")
            };
        }


        public static async Task<Stream> SendRequestAsync()
        {
            if (httpClient == null)
            {
                InitHttpClient();
            }

            CancellationToken ct = new CancellationToken();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/Suppliers");           
            
            var httpResponse = await httpClient.SendAsync(httpRequestMessage, ct);
            var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return responseStream;
        }

        public static async Task<T> GetRequestResult<T>()
        {
            Stream resultStream = await SendRequestAsync();

            T result = default(T); // JsonConvert.DeserializeObject<T>(resultStream.ReadToEnd());

            return result;
        }
    }
}
