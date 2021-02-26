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

namespace блокнот
{
    public partial class Form1 : Form
    {
        public string filename;
        public bool isFileChanged;

        public int fontsize = 0;
        public System.Drawing.FontStyle fs = FontStyle.Regular;

        public FontSettings FontSetts;
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            filename = "";
            isFileChanged = false;
            UpdateTextWithTitle();
            
        }

        public void CreateNewDocument(object sender, EventArgs e)
        {
            SaveUnsavedFile();
            richTextBox1.Text = string.Empty;
            filename = "";
            isFileChanged = false;
            UpdateTextWithTitle();
        }
        public void OpenFile(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                openFileDialog1.FileName = "";
            SaveUnsavedFile();
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                    filename = openFileDialog1.FileName;
                    isFileChanged = false;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл.");
                }                
            }
            UpdateTextWithTitle();
        }

        public void SaveFile (string _filename)
        {
            if (_filename == "")
            {
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    _filename = saveFileDialog1.FileName;
                }
            }
            try
            {
                StreamWriter sw = new StreamWriter(_filename + ".txt");
                sw.Close();
                filename = _filename;
                isFileChanged = false;
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить файл.");
            }
            UpdateTextWithTitle();
        }
        public void Save (object sender, EventArgs e)
        {
            SaveFile(filename);
        }
        public void SaveAs(object sender, EventArgs e)
        {
            SaveFile("");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
          
         
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (isFileChanged)
            {
                this.Text = this.Text.Replace('*' , ' ');
                isFileChanged = true;
                this.Text = "*" + this.Text;
            }   
        }
        public void UpdateTextWithTitle()
        {
            if(filename!="")
            this.Text = filename + " - Блокнот";
            else this.Text = filename + "Новый текстовый документ - Блокнот";
        }
        public void SaveUnsavedFile()
        {
            if(isFileChanged)
            {
                DialogResult result = MessageBox.Show("Сохранить изменения в файле?", "Сохранение файла", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.Yes)
                {
                    SaveFile(filename);
                }
            }
        }
        public void CopyText()
        {
            Clipboard.SetText(richTextBox1.SelectedText);             
        }
        public void CutText()
        {
            Clipboard.SetText(richTextBox1.Text.Substring(richTextBox1.SelectionStart, richTextBox1.SelectionLength));
            richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart, richTextBox1.SelectionLength);
        }
        public void PasteText()
        {
            Clipboard.GetText();
        }

        private void OnCutClick(object sender, EventArgs e)
        {
            CutText();
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            CopyText();
        }

        private void OnPasteClick(object sender, EventArgs e)
        {
            PasteText();
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUnsavedFile();
        }

        private void OnFontClick(object sender, EventArgs e)
        {
            FontSetts = new FontSettings();
            FontSetts.Show();
        }

        private void OnFocus(object sender, EventArgs e)
        {
            
        }
    }
}