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
                Filedialog.Title = "�t�@�C����I�����Ă�������";
                Filedialog.Filter = "���ׂẴt�@�C��(*.*)|*.*";
                Filedialog.Multiselect = false;
                Filedialog.FileName = "";

                if (Filedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        textBox1.Text = Filedialog.FileName;
                        textBox2.Text = System.IO.File.ReadAllText(Filedialog.FileName);
                        MessageBox.Show("�I�������t�@�C���F" + Filedialog.FileName, "���", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"�t�@�C���ǂݍ��ݒ��ɃG���[���������܂����B\n{ex.Message}", "�G���[", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show("�t�@�C���I�����L�����Z������܂����B", "���", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            Description = "�c���^��",
                        },
                        ["day"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "����",
                        },
                        ["participants"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "�Q����",
                        },
                        ["subject"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "�c��",
                        },
                        ["determining_matter"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "���莖��",
                        },
                        ["meeting_detail"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "�c�_���e",
                        },
                        ["todo"] = new Schema()
                        {
                            Type = DataType.String,
                            Description = "TODO�i����A�N�V�����j",
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
                    Contents = textBox2.Text + "\n" + "��L���X�L�[�}�ɂ��������ċc���^���쐬����"
                });

                textBox3.Text = response2.GetText();
            }
            catch (Exception ex)
            {
                Console.WriteLine("�G���[���������܂����F" + ex.Message);
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
