using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class EnterName : Form
    {
        MainForm form;
        public DialogResult result;
        public EnterName(string lastName, MainForm main)
        {
            form = main;
            InitializeComponent();
            textBox1.Text = lastName;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            result = DialogResult.OK;
            this.Close();
        }

        private void EnterName_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
