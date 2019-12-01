using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Configuration;

namespace HrkMessage
{
    public partial class Form1 : Form
    {
        public int fileCnt = 0;
        //public const string CURRENT_DIRECTORY = Environment.CurrentDirectory;
        //public const string CURRENT_DIRECTORY = @"\\koidrive\Files\03.業務共通\02.マーケティングIT企画本部\09.プロジェクトドキュメント\1000.福利厚生\H30年度\0011.サポート・ライセンス切れー.net1.1対応\90.その他\90.個人\廣木\HrkMessage";
        public string CURRENT_DIRECTORY = ConfigurationManager.AppSettings["CurrentPath"];
        private System.Timers.Timer timer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
            ReadLogFile();
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsed_TimersTimer);
            this.timer.Interval = 1000;
            this.timer.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                return;
            }

            PutTextSaveFile();
        }

        // KeyDown イベントのイベントハンドラ
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Alt+Enter キーが押されていたら
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }

                PutTextSaveFile();
            }
        }

        // TextBoxに入力された文字列をリッチテキストに出力して、ファイルに保存する処理
        private void PutTextSaveFile()
        {
            Font baseFont = richTextBox1.SelectionFont;
            Font fnt = new Font(baseFont.FontFamily, baseFont.Size, baseFont.Style | FontStyle.Bold);

            this.richTextBox1.SelectionFont = fnt;
            this.richTextBox1.SelectedText += Environment.MachineName + " - " + DateTime.Now + "--------------------" + Environment.NewLine;

            this.richTextBox1.SelectionFont = baseFont;
            this.richTextBox1.SelectedText += textBox1.Text + Environment.NewLine;

            string str = Environment.MachineName + " - " + DateTime.Now + "--------------------" + Environment.NewLine;
            str += textBox1.Text + Environment.NewLine;
            string filePath = CURRENT_DIRECTORY + @"\msglog\";
            string fileName = Environment.MachineName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllText(filePath + fileName, str);
            fileCnt++;
            textBox1.Clear();
        }

        private void ReadLogFile()
        {
            String filePath = CURRENT_DIRECTORY + @"\msglog\";
            if (!Directory.Exists(filePath))
            {
                return;
            }
            //起動時は全ファイル表示
            if(fileCnt == 0)
            {
                var files = Directory.GetFiles(filePath).OrderBy(f => File.GetCreationTime(f));
                fileCnt = files.Count();
                string readText;
                foreach (var f in files)
                {
                    readText = File.ReadAllText(f);
                    this.richTextBox1.SelectedText += readText;
                }
                this.richTextBox1.ScrollToCaret();
            }
            else
            {
                var files = Directory.GetFiles(filePath).OrderBy(f => File.GetCreationTime(f)).ToArray();
                int cnt = files.Count();
                if(fileCnt != cnt)
                {
                    var dst = new string[cnt - fileCnt];
                    Array.Copy(files, fileCnt, dst, 0, cnt - fileCnt);
                    string readText;
                    foreach (var f in dst)
                    {
                        readText = File.ReadAllText(f);
                        this.richTextBox1.SelectedText += readText;
                    }
                    this.richTextBox1.ScrollToCaret();
                    fileCnt = cnt;
                }
                return;
            }
            
        }
        private async void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                ReadLogFile();
            });
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
