using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace блокнот
{
    public partial class FontSettings : Form
    {
        public int fontsize = 0;
        public System.Drawing.FontStyle fs = FontStyle.Regular;
        public FontSettings()
        {
            InitializeComponent();
            FontBox.SelectedItem = FontBox.Items[0];
            StyleBox.SelectedItem = StyleBox.Items[0];
        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), ExampleText.Font.Style);
            fontsize = int.Parse(FontBox.SelectedItem.ToString());
        }

        private void OnStyleChange(object sender, EventArgs e)
        {
            switch (StyleBox.SelectedItem.ToString())
            {
                case "Обычный":
                    ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), FontStyle.Regular);
                    break;
                case "Курсив":
                    ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), FontStyle.Italic);
                    break;
                case "Полужирный":
                    ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), FontStyle.Bold);
                    break;
                case "Линия по середине":
                    ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), FontStyle.Strikeout);
                    break;
                case "Подчёркивание":
                    ExampleText.Font = new Font(ExampleText.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()), FontStyle.Underline);
                    break;
            }
            fs = ExampleText.Font.Style;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}