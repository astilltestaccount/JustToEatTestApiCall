using Xunit;
using RichardSzalay.MockHttp;
using JustToEat.CallApi;
using JustToEatTest.MyMock;

namespace JustToEatTest
{
    public class CallRestarauntApiTest
    {
        [Fact]
        public async void ProcessResponse_GoodResponse_Test()
        {
            var mockHttp = new MockHttpMessageHandler();
            var restarauntHttpResponseHandler = new RestarauntHttpResponseHandler();
            string json = @"{'Restaurants': [{'Badges': [],'Score': 17.0483932,'DriveDistance': 1.7,'DriveInfoCalculated': true,'NewnessDate': '2011-06-17T10:53:05Z','DeliveryMenuId': 46538,'DeliveryOpeningTime': '2018-04-09T10:30:00Z','DeliveryCost': 0,'MinimumDeliveryValue': 10,'DeliveryTimeMinutes': null,'DeliveryWorkingTimeMinutes': 45,'OpeningTime': '/Date(1523356200000+0000)/','OpeningTimeIso': '2018-04-10T10:30:00Z','SendsOnItsWayNotifications': false,'RatingAverage': 5.26,'Latitude': 51.398557,'Longitude': -0.076405,'Tags': [],'ScoreMetadata': {},'Id': 13620,'Name': 'Pizza Plus Pizza','Address': '2 High Street','Postcode': 'SE25 6EP','City': 'South Norwood','CuisineTypes': [{'Id': 82,'Name': 'Pizza','SeoName': 'pizza'},{'Id': 79,'Name': 'Chicken','SeoName': 'chicken'}],'Url': '','IsOpenNow': true,'IsPremier': false,'IsSponsored': true,'IsTemporaryBoost': false,'SponsoredPosition': 0,'IsNew': false,'IsTemporarilyOffline': false,'ReasonWhyTemporarilyOffline': '','UniqueName': 'pizzapluspizzase25','IsCloseBy': false,'IsHalal': true,'IsTestRestaurant': false,'DefaultDisplayRank': 0,'IsOpenNowForDelivery': true,'IsOpenNowForCollection': true,'RatingStars': 5.5,'Logo': [{'StandardResolutionURL': 'http://d30v2pzvrfyzpo.cloudfront.net/uk/images/restaurants/13620.gif'}],'Deals': [],'NumberOfRatings': 948},{'Badges': [],'Score': 9.47649,'DriveDistance': 1.8,'DriveInfoCalculated': true,'NewnessDate': '2017-04-11T14:58:17Z','DeliveryMenuId': 241108,'DeliveryOpeningTime': '2018-04-09T10:30:00Z','DeliveryCost': 0,'MinimumDeliveryValue': 12,'DeliveryTimeMinutes': null,'DeliveryWorkingTimeMinutes': 45,'OpeningTime': '/Date(1523356200000+0000)/','OpeningTimeIso': '2018-04-10T10:30:00Z','SendsOnItsWayNotifications': false,'RatingAverage': 4.9,'Latitude': 51.415734,'Longitude': -0.053033,'Tags': [],'ScoreMetadata': {},'Id': 67608,'Name': 'Bella Luna Pizzeria','Address': '115 High Street','Postcode': 'SE20 7DT','City': 'London','CuisineTypes': [{'Id': 82,'Name': 'Pizza','SeoName': 'pizza'},{'Id': 27,'Name': 'Italian','SeoName': 'italian'}],'Url': '','IsOpenNow': true,'IsPremier': false,'IsSponsored': true,'IsTemporaryBoost': false,'SponsoredPosition': 0,'IsNew': false,'IsTemporarilyOffline': false,'ReasonWhyTemporarilyOffline': '','UniqueName': 'bellaluna-pizzeria-se20','IsCloseBy': false,'IsHalal': false,'IsTestRestaurant': false,'DefaultDisplayRank': 1,'IsOpenNowForDelivery': true,'IsOpenNowForCollection': true,'RatingStars': 5,'Logo': [{'StandardResolutionURL': 'http://d30v2pzvrfyzpo.cloudfront.net/uk/images/restaurants/67608.gif'}],'Deals': [{'Description': '20% off when you spend £25','DiscountPercent': '20','QualifyingPrice': '25.00'}],'NumberOfRatings': 201}]}";

            mockHttp.When("https://public.je-apis.com/*")
                    .Respond("application/json", json);

            var client = mockHttp.ToHttpClient();

            var response = await client.GetAsync("https://public.je-apis.com/restaraunts?q=1234");
            MockOut output = new MockOut();

            restarauntHttpResponseHandler.ProcessResponseMessage(response, output);
            var GoodString = "Name:Pizza Plus Pizza\tTypes of Food:Pizza, Chicken\tRating: 5.26\nName:Bella Luna Pizzeria\tTypes of Food:Pizza, Italian\tRating: 4.9\n";
            Assert.Equal(GoodString, output._stringBuilder.ToString());
        }

        [Fact]
        public async void ProcessResponse_BadResponse_Test()
        {
            var mockHttp = new MockHttpMessageHandler();
            var restarauntHttpResponseHandler = new RestarauntHttpResponseHandler();
            string json = "@{'bad' : 'true'}";
            mockHttp.When("https://public.je-apis.com/bad")
                    .Respond("application/json", json); // Respond with JSON

            var client = mockHttp.ToHttpClient();

            var response = await client.GetAsync("https://public.je-apis.com/restaraunts?q=1234");
            MockOut output = new MockOut();

            restarauntHttpResponseHandler.ProcessResponseMessage(response, output);
            var badString = "Error occurred, the status code is: NotFound";
            Assert.Equal(badString, output._stringBuilder.ToString());
        }

        [Fact]
        public async void ProcessResponse_EmptyResponse_Test()
        {
            var mockHttp = new MockHttpMessageHandler();
            var restarauntHttpResponseHandler = new RestarauntHttpResponseHandler();
            string json = @"{'Restaurants': []}";
            mockHttp.When("https://public.je-apis.com/*")
                    .Respond("application/json", json);

            var client = mockHttp.ToHttpClient();

            var response = await client.GetAsync("https://public.je-apis.com/restaraunts?q=1234");
            MockOut output = new MockOut();

            restarauntHttpResponseHandler.ProcessResponseMessage(response, output);

            var badString = "Unfornunately, system can't find any restaraunt for this code.";
            Assert.Equal(badString, output._stringBuilder.ToString());
        }
    }
}
