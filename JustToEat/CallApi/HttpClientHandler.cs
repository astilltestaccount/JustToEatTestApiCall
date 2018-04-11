using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace JustToEat.CallApi
{

    public class HttpClientHandler : IHttpHandler
    {
        private HttpClient _client = new HttpClient();
        private string _host;

        public HttpClientHandler()
        {

            var auth = Program.Configuration["auth:basic:password"];
            if (string.IsNullOrEmpty(auth)) throw new Exception("Wrong Authorization. Check your appsetting.json");
            auth = "Basic " + auth;

            _host = Program.Configuration["host"];
            if (string.IsNullOrEmpty(_host)) throw new Exception("Wrong host parameter.Check your appsetting.json");

            _client.BaseAddress = new Uri(_host);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", auth);
            _client.DefaultRequestHeaders.Add("Accept-Language", "en-GB");
            _client.DefaultRequestHeaders.Add("Accept-Tenant", "uk");

        }

        public HttpResponseMessage Get(string url)
        {
            return GetAsync(url).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> OptionAsync(string controler="")
        {
            return await _client.SendAsync(new HttpRequestMessage(HttpMethod.Options, _host+controler));

        }

        public string GetAvailableMethodsOnController(string controller="")
        {
            var AllowMethods = "";
            var headers = OptionAsync(controller).Result.Headers;
            IEnumerable<string> values;
            if (headers.TryGetValues("Access-Control-Allow-Methods", out values))
            {
                 AllowMethods = values.First();
            }
            return AllowMethods;
        }

    }
}
