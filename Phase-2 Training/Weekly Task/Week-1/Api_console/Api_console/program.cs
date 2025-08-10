using System;
using System.Net.Http;
using System.Threading.Tasks;


class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Fetching records");


        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");


                response.EnsureSuccessStatusCode();


                string data = await response.Content.ReadAsStringAsync();


                Console.WriteLine("Data fetched");
                Console.WriteLine(data);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }


        Console.WriteLine("Done.");
    }
}
