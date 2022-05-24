// See https://aka.ms/new-console-template for more information
using Lecture.Console.Models;
using Newtonsoft.Json;
using RestSharp;

Console.WriteLine("Hello, World!");
//HttpClientTest();
RestSharpTest();


void HttpClientTest()
{
    var httpClient = new HttpClient();

    var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7087/WeatherForecast");
    //request.Content = new FormUrlEncodedContent();
    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

    if (response != null)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var list = JsonConvert.DeserializeObject<List<WeatherForecastModel>>(responseText);

            foreach (var item in list)
            {
                Console.WriteLine($"{item.Date}  {item.TemperatureC}");
            }
        }
        else
        {
            var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(responseText);
        }
    }
    else
    {
        Console.WriteLine("response == null");
    }

    Console.ReadLine();
}

void RestSharpTest()
{
    var restClient = new RestClient();

    var request = new RestRequest()
    {
        Method = Method.Post,
        Resource = "https://localhost:7087/WeatherForecast",
        RequestFormat = DataFormat.None,    // 보내는 메시지가 JSON형식을 의미
    };
    request.AddQueryParameter("addDays", 3);
    //request.AddParameter(); // Form or Body를 통해 전송하는 방식

    var response = restClient.ExecuteAsync(request).GetAwaiter().GetResult();

    if (response != null)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var responseText = response.Content;

            //{
            //    var list = JsonConvert.DeserializeObject<List<WeatherForecastModel>>(responseText);

            //    foreach (var item in list)
            //    {
            //        Console.WriteLine($"{item.Date}  {item.TemperatureC}");
            //    }
            //}

            {
                var item = JsonConvert.DeserializeObject<WeatherForecastModel>(responseText);
                Console.WriteLine($"{item.Date}  {item.TemperatureC}");
            }
        }
        else
        {
            var responseText = response.Content;
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(responseText);
        }
    }
    else
    {
        Console.WriteLine("response == null");
    }

    Console.ReadLine();
}