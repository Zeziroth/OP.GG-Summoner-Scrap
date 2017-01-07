using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OP.GG_Scrap
{

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private readonly string VERSION = "v1.0";
        internal int threadCount = 0;
        internal int maxThreads = 1;
        public bool started = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            Core.Init(this);
            metroComboBox1.SelectedIndex = 0;
            this.Text += " " + VERSION;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            switch (metroButton1.Text)
            {
                case "Stop":
                    started = false;
                    Core.CloseThreads();
                    numericUpDown1.Enabled = true;
                    metroComboBox1.Enabled = true;
                    Scraper.queueNames = new List<string>();
                    metroButton1.Text = "Scrap";
                    break;

                case "Scrap":
                    if (Core.isValidName(metroTextBox1.Text, metroComboBox1.SelectedIndex))
                    {
                        metroButton1.Text = "Stop";
                        numericUpDown1.Enabled = false;
                        metroComboBox1.Enabled = false;
                        maxThreads = (int)numericUpDown1.Value + 1;
                        Core.RunThread(Scraper.Master);
                        Core.RunThread(Scraper.LoadProfile, metroTextBox1.Text);
                        System.Threading.Thread.Sleep(1000);
                        started = true;
                        return;
                    }
                    MessageBox.Show("Please use a valid name for the chosen region...");
                    break;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.CloseThreads();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
           if (Scraper.GetSummoners().Count == 0)
            {
                MessageBox.Show("You have not grabbed any user yet!");
                return;
            }
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = metroTextBox1.Text + ".txt";
            save.Filter = "Text File | *.txt";
            if (save.ShowDialog() == DialogResult.OK)
            {
                string allNames = String.Join(Environment.NewLine, Scraper.GetSummoners().ToArray());
                System.Text.Encoding enc = new UTF8Encoding();

                if (!File.Exists(save.FileName))
                {
                    File.WriteAllText(save.FileName, allNames, enc);
                }
                else
                {
                    MessageBox.Show("Cannot save file." + Environment.NewLine + save.FileName + Environment.NewLine + "... already exists!");
                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Scraper.summonerNames = new List<string>();
            Scraper.scannedNames = new List<string>();
            counter_lbl.Text = "0";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coder: GlumiChan" + Environment.NewLine + "Version: " + VERSION);
            System.Diagnostics.Process.Start("http://boehmer.pro/");
        }
    }
}
