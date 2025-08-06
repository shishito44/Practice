// See https://aka.ms/new-console-template for more information
using GemiNet;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

string message = configuration.GetSection("Secrets:Message").Value ?? "Failed";

using var ai = new GoogleGenAI
{
    ApiKey = message
};


var response = await ai.Models.GenerateContentAsync(new()
{
    Model = Models.Gemini2_0Flash,    // models/gemini-2.0-flash
    Contents = "こんにちは"
});

Console.WriteLine(response.GetText());
