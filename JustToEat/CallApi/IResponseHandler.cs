using System;
using System.Net.Http;
namespace JustToEat
{
    interface IResponseHandler
    {
        void ProcessResponseMessage(HttpResponseMessage response,Output output);
    }
}
