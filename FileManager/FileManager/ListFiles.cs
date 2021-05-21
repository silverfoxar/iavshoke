using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace FileManager
{
    class ListFiles : ListBox
    {
        public TextBox addressText;
        public ListFiles() : base()
        {
            SelectionMode = SelectionMode.MultiExtended;
            Dock = DockStyle.Fill;
        }
        public void ShowMyComputer()
        {
            addressText.Text = "";
            Items.Clear();
            string[] Drives = Environment.GetLogicalDrives();
            foreach (string s in Drives)
            {
                Items.Add(s);
            }
        }
        public void Open()
        {
            if (SelectedIndex != -1)
            {
                OpenDirectory(addressText.Text + Convert.ToString(Items[SelectedIndex]));
            }
        }
        public void GoBack()
        {
            if (addressText.Text != "")
            {
                int x = addressText.Text.LastIndexOf("\\");
                string newDirectory = addressText.Text.Remove(x, addressText.Text.Length - x); 
                x = newDirectory.LastIndexOf("\\");
                if (x != -1)
                {
                    newDirectory = newDirectory.Remove(x, newDirectory.Length - x);
                    if (newDirectory != "")
                    {
                        OpenDirectory(newDirectory);
                    }
                    else
                    {
                        ShowMyComputer();
                    }
                }
                else
                {
                    ShowMyComputer();
                }
                UpdateContent();
            }
            else
            {
                ShowMyComputer();
            }
        }
        public void OpenDirectory(string directory)
        {
            if (directory !="")
            {
                try
                {
                    Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    foreach (var item in dir.GetDirectories())
                    {
                        Items.Add(item);
                    }
                    foreach (var item in dir.GetFiles())
                    {
                        Items.Add(item);
                    }
                    if (directory.LastIndexOf("\\") != directory.Length-1)
                    {
                        addressText.Text = directory + '\\';
                    }
                    else
                    {
                        addressText.Text = directory;
                    }
                }
                catch
                {
                    try
                    {
                        Process.Start(directory);
                        UpdateContent();
                    }
                    catch
                    {
                        ShowMyComputer();
                    }
                }
            }
            else
            {
                ShowMyComputer();
            }
        }
        public void UpdateContent()
        {
            OpenDirectory(addressText.Text);
        }
    }
}