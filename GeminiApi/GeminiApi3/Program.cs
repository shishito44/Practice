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
;
var minutesplompt = "C:/repo/Practice/textfile/minutesplompt.txt";
string? meetingPath;
string? meetingtext;

while (true)
{
    try
    {
        Console.WriteLine("ファイル名を入力してください(finを押すと終了)");
        meetingPath = Console.ReadLine();
        if (string.IsNullOrEmpty(meetingPath))
        {
            continue;
        }
        if (meetingPath == "fin")
        {
            Console.WriteLine("プログラムを終了します");
            return;
        }
        meetingtext = System.IO.File.ReadAllText(meetingPath);
        Console.WriteLine(meetingtext);
        break;
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("ファイルが見つかりません。");
    }
    catch (Exception e)
    {
        Console.WriteLine("エラーが発生しました：" + e.Message);
    }
}

var minutestext = System.IO.File.ReadAllText(minutesplompt);
Console.WriteLine(minutestext);

var response2 = await ai.Models.GenerateContentAsync(new()
{
    Model = Models.Gemini2_0Flash,    // models/gemini-2.0-flash
    Contents = meetingtext + "\n" + minutestext
});

Console.WriteLine(response2.GetText());