using System;
using System.Threading.Tasks;
using System.Net.Http;


namespace JustToEat.CallApi
{
    public interface IHttpHandler
    {
        HttpResponseMessage Get(string url);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
