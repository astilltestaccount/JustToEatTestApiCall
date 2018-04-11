using System;
using System.Net.Http;
using System.Text;
using JustToEat.Model.RestarauntModel;

namespace JustToEat.CallApi
{
    public class RestarauntHttpResponseHandler : IResponseHandler
    {
        public async void ProcessResponseMessage(HttpResponseMessage response,Output output)
        {
            if (response.IsSuccessStatusCode)
            {
                RootJsonObject restaraunts = await response.Content.ReadAsAsync<RootJsonObject>();
                output.Write(ParseRestarauntJsonObject(restaraunts));
            }
            else
            {
                output.Write(
                    String.Format("Error occurred, the status code is: {0}",
                                  response.StatusCode)
            );
            }
        }

        private string ParseRestarauntJsonObject(RootJsonObject restaraunts)
        {
            StringBuilder OutputMessage = new StringBuilder();
            if (restaraunts.Restaurants.Count == 0)
            {
                OutputMessage.Append("Unfornunately, system can't find any restaraunt for this code.");
                return OutputMessage.ToString();
            }
            foreach (var restaraunt in restaraunts.Restaurants)
            {
                StringBuilder foodType = new StringBuilder();
                foodType.Append(restaraunt.CuisineTypes[0].Name);
                for (int i = 1; i < restaraunt.CuisineTypes.Count; i++)
                {
                    foodType.Append(", " + restaraunt.CuisineTypes[i].Name);
                }
                OutputMessage.Append(String.Format("Name:{0}\tTypes of Food:{1}\tRating: {2}\n", restaraunt.Name, foodType, restaraunt.RatingAverage));
            }

            return OutputMessage.ToString();
        }
    }
}
