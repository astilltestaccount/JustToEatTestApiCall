using System;
using System.Net.Http;
using JustToEat;
namespace JustToEat.CallApi
{
    public class CallRestarauntAPI
    {
        HttpClientHandler _httpClientHandler = new HttpClientHandler();
        RestarauntHttpResponseHandler _httpResponseHandler = new RestarauntHttpResponseHandler();
        private readonly string _controller = "restaurants";
        private string _AvailableMethods;

        public CallRestarauntAPI()
        {
            _AvailableMethods = _httpClientHandler.GetAvailableMethodsOnController(_controller);
        }
        public void Get(string query)
        {
            if (_AvailableMethods.IndexOf("GET", StringComparison.CurrentCulture) == -1)
            {
                var ErrorMsg = "This controller doesn't support HttpMethod GET. Available message: " + _AvailableMethods;
                throw new Exception(ErrorMsg);
            }
            string queryString = _controller + "?q=" + query;
            ProcessResponse(_httpClientHandler.Get(queryString));

        }

        public void ProcessResponse(HttpResponseMessage response)
        {
            _httpResponseHandler.ProcessResponseMessage(response, new SystemOutput());
        }

    }


}
