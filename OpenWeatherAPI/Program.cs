using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeatherAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // grab appsettings.json file and read text it contains
            var appsetingsText =File.ReadAllText("appsettings.json");
            
            //Console.WriteLine(appsetingsText);
            // parse the json into an object
            var apiKey = JObject.Parse(appsetingsText)["key"].ToString();
            
            //prompt for zip code from user
            Console.WriteLine("Please enter a zip code:");
            
            // store it
            var zip = Console.ReadLine();

            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?zip={zip}&appid={apiKey}";
            
            // send GET request to url created 
            var client = new HttpClient();
            var response = client.GetAsync(weatherUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(result);
                
                var cityName = json["name"].ToString();
                var condition = json["weather"][0]["description"].ToString();
                var tempKelvin = json["main"]["temp"].ToObject<double>();
                var tempFahrenheit = (tempKelvin - 273.15) * 9/5 + 32; // needed to convert to fahrenheit 
                
                
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("\uD83C\uDF0D Weather Report:"); // 🌍
                Console.WriteLine($"\uD83D\uDDFA Location: {cityName}"); // 📍
                Console.WriteLine($"🌡 Temperature: {tempFahrenheit:F1}°F");
                Console.WriteLine($"\u2601 Condition: {condition}"); // ☁
                
            }
            else
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8; // jetbrains needed this code input for me to use the fun unicode (had to look up)
                Console.WriteLine("\u274C Error fetching weather data. Please check your API key and ZIP code."); //red X
            }
            // Tested finished


        }
    }
}
