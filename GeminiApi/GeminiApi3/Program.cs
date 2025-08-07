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


var responseSchema = new Schema()
{
    Type = DataType.Object,
    Properties = new Dictionary<string, Schema>()
    {
        ["title"] = new Schema()
        {
            Type = DataType.String,
            Description = "議事録名",
        },
        ["day"] = new Schema()
        {
            Type = DataType.String,
            Description = "日時",
        },
        ["participants"] = new Schema()
        {
            Type = DataType.String,
            Description = "参加者",
        },
        ["subject"] = new Schema()
        {
            Type = DataType.String,
            Description = "議題",
        },
        ["determining_matter"] = new Schema()
        {
            Type = DataType.String,
            Description = "決定事項",
        },
        ["meeting_detail"] = new Schema()
        {
            Type = DataType.String,
            Description = "議論内容",
        },
        ["todo"] = new Schema()
        {
            Type = DataType.String,
            Description = "TODO（次回アクション）",
        },




    },
    Required = ["title", "day", "participants", "subject", "determining_matter", "meeting_detail", "todo"]
};


var generationConfig = new GenerationConfig()
{
    ResponseSchema = responseSchema,
    ResponseMimeType = "application/json",
};


var response2 = await ai.Models.GenerateContentAsync(new()
{
    GenerationConfig = generationConfig,
    Model = Models.Gemini2_0Flash,    // models/gemini-2.0-flash
    Contents = meetingtext + "\n" + "上記をスキーマにしたがって議事録を作成して"
});

Console.WriteLine(response2.GetText());