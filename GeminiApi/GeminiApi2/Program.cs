// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using GemiNet;
using Microsoft.Extensions.Configuration;
using File = GemiNet.File;

IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

string key = configuration.GetSection("Secrets:Message").Value ?? "Failed";

using var ai = new GoogleGenAI
{
    ApiKey = key
};

string meetingplompt = "C:/repo/Practice/textfile/meetingplompt.txt";
string minutesplompt = "C:/repo/Practice/textfile/minutesplompt.txt";
string filePath = "C:/repo/Practice/textfile/newfile.txt";
string? meetingtext = null;
string? minutestext = null;

meetingtext = System.IO.File.ReadAllText(meetingplompt);
Console.WriteLine(meetingtext);

var response1 = await ai.Models.GenerateContentAsync(new()
{
    Model = Models.Gemini2_0Flash,    // models/gemini-2.0-flash
    Contents = meetingtext
});
string? minutesfile = response1.GetText();

using (StreamWriter writer = new StreamWriter(filePath,false))
{
    writer.WriteLine(minutesfile);
}
Console.WriteLine(minutesfile);

minutestext = System.IO.File.ReadAllText(minutesplompt);
Console.WriteLine(minutestext);

var response2 = await ai.Models.GenerateContentAsync(new()
{
    Model = Models.Gemini2_0Flash,    // models/gemini-2.0-flash
    Contents = minutesfile + "\n" + minutestext
});

Console.WriteLine(response2.GetText());