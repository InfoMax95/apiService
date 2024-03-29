﻿// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Net.Http.Json;
using tracking.client;

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7171");
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

HttpResponseMessage response = await client.GetAsync("api/Posts");
response.EnsureSuccessStatusCode();

if(response.IsSuccessStatusCode)
{
    var posts = await response.Content.ReadFromJsonAsync<IEnumerable<PostDto>>();
    foreach (var post in posts)
    {
        Console.WriteLine(post.Title);
    }
} else
{
    Console.WriteLine("No results");
}

Console.Read();