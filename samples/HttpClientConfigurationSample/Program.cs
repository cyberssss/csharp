﻿using IPinfo;
using IPinfo.Models;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
  class Program
  {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\nSample for changing configuration for http client");

            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            // to use this sample, add your IPinfo Access Token to environment variable
            // named "IPINFO_TOKEN", or initialize your token string directly.
            string? token = Environment.GetEnvironmentVariable("IPINFO_TOKEN");        

            TimeSpan timeOut = TimeSpan.FromSeconds(5);
            HttpClient httpClient = new HttpClient();

            if(token is not null)
            {
              // initializing IPinfo client
              IPinfoClient client = new IPinfoClient.Builder()
                .AccessToken(token, configuration)  // pass your token string and configuraton(optional)
                .HttpClientConfig(config => config
                  .Timeout(timeOut) // pass timeout as TimeSpan
                  .HttpClientInstance(httpClient)) // pass your own HttpClient instance
                .Build();

              string ip = PromptHelper();              
              while(!ip.Equals("0"))
              {
                // making API call
                IPResponse ipResponse = await client.IPApi.GetDetailsAsync(ip);

                Console.WriteLine($"IPResponse.Bogon: {ipResponse.Bogon}");
                Console.WriteLine($"IPResponse.IP: {ipResponse.IP}");
                if (!ipResponse.Bogon)
                {
                  if (ipResponse.IsCrawler.HasValue)
                  {
                     Console.WriteLine($"IPResponse.IsCrawler: {ipResponse.IsCrawler}");                     
                  }
                  Console.WriteLine($"IPResponse.City: {ipResponse.City}");
                  Console.WriteLine($"IPResponse.Company.Name: {ipResponse.Company?.Name}");
                  Console.WriteLine($"IPResponse.Country: {ipResponse.Country}");
                  Console.WriteLine($"IPResponse.CountryName: {ipResponse.CountryName}");
                }
                ip = PromptHelper();
              }
            }
            else
            {
              Console.WriteLine("Set your access token as IPINFO_TOKEN in environment variables in order to run this sample code. You can also set your token string in the code manually.");
              return;
        }
    }

    private static string PromptHelper()
    {
        Console.WriteLine("\nOptions:");
        Console.WriteLine("-Enter 0 to quit");
        Console.WriteLine("-Enter ip address:");
        return Console.ReadLine()??"";
    }
  }
}
