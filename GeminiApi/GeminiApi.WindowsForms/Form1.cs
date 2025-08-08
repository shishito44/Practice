using GemiNet;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Reflection;
using File = GemiNet.File;


namespace GeminiApi.WinddowsForms
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var Filedialog = new OpenFileDialog())
            {
                Filedialog.Title = "ファイルを選択してください";
                Filedialog.Filter = "すべてのファイル(*.*)|*.*";
                Filedialog.Multiselect = false;
                Filedialog.FileName = "";

                if (Filedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        textBox1.Text = Filedialog.FileName;
                        textBox2.Text = System.IO.File.ReadAllText(Filedialog.FileName);
                        MessageBox.Show("選択したファイル：" + Filedialog.FileName, "情報", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ファイル読み込み中にエラーが発生しました。\n{ex.Message}", "エラー", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show("ファイル選択がキャンセルされました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Form1>().Build();

                string key = configuration.GetSection("Secrets:Message").Value ?? "Failed";

                using var ai = new GoogleGenAI
                {
                    ApiKey = key
                };
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
                    Required =
                    [
                        "title", "day", "participants", "subject", "determining_matter", "meeting_detail", "todo"
                    ]
                };


                var generationConfig = new GenerationConfig()
                {
                    ResponseSchema = responseSchema,
                    ResponseMimeType = "application/json",
                };

                var response2 = await ai.Models.GenerateContentAsync(new()
                {
                    GenerationConfig = generationConfig,
                    Model = Models.Gemini2_0Flash, // models/gemini-2.0-flash
                    Contents = textBox2.Text + "\n" + "上記をスキーマにしたがって議事録を作成して"
                });

                textBox3.Text = response2.GetText();
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラーが発生しました：" + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
