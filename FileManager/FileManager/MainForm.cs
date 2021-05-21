using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Ionic.Zip;
namespace FileManager
{
    public partial class MainForm : Form
    {
        ListFiles ActiveList;
        List<string> selectedFiles;
        public MainForm()
        {
            InitializeComponent();
            listFiles1.addressText = textBox1;
            listFiles1.ShowMyComputer();
            listFiles2.addressText = textBox2;
            listFiles2.ShowMyComputer();
            ActiveList = listFiles1;
            selectedFiles = new List<string>(10);
            listFiles1.ContextMenuStrip = contextMenuStrip1;
            listFiles2.ContextMenuStrip = contextMenuStrip1;
        }

        private void listFiles2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listFiles1.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listFiles1.GoBack();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            listFiles1.OpenDirectory(textBox1.Text);
        }

        private void listFiles2_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            listFiles2.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listFiles2.GoBack();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                listFiles2.OpenDirectory(textBox2.Text);
        }

        private void listFiles1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listFiles2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void buttonNewFolder_Click(object sender, EventArgs e)
        {
            NewFolder();
        }
        void NewFolder()
        {
            EnterName nameForm = new EnterName("Новая папка", this);
            nameForm.ShowDialog();
            if (nameForm.result == DialogResult.OK)
            {
                Directory.CreateDirectory(Path.Combine(ActiveList.addressText.Text, nameForm.textBox1.Text));
                ActiveList.UpdateContent();
            }
        }
        private void listFiles1_MouseClick(object sender, MouseEventArgs e)
        {
            ActiveList = listFiles1;
        }

        private void listFiles2_MouseClick(object sender, MouseEventArgs e)
        {
            ActiveList = listFiles2;
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult del = MessageBox.Show("Вы уверены?", "Удалить?", MessageBoxButtons.OKCancel);
            if (del == DialogResult.OK)
            {
                List<string> files = new List<string>();
                string pathDirectory = ActiveList.addressText.Text;
                foreach (var f in ActiveList.SelectedItems)
                {
                    files.Add(f.ToString());
                }
                await delete(files);
            }
        }
        async Task delete(List<string> files)
        {
            foreach (var f in ActiveList.SelectedItems)
            {
                try
                {
                    Directory.Delete(ActiveList.addressText.Text + f, true);
                }
                catch
                {
                    File.Delete(ActiveList.addressText.Text + f);
                }
            }
            ActiveList.UpdateContent();
        }
        private void buttonRename_Click(object sender, EventArgs e)
        {
            object obj = ActiveList.SelectedItem;
            if (obj != null)
            {
                EnterName nameForm = new EnterName(obj.ToString(), this);
                nameForm.ShowDialog();
                if (nameForm.result == DialogResult.OK)
                {
                    string newPath = Path.Combine(ActiveList.addressText.Text, nameForm.textBox1.Text);
                    try
                    {
                        if (File.Exists(newPath))
                        {
                            MessageBox.Show("Файл с таким именем уже существует", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            File.Move(Path.Combine(ActiveList.addressText.Text, ActiveList.SelectedItem.ToString()), newPath);
                        }

                    }
                    catch
                    {
                        if (Directory.Exists(newPath))
                        {
                            MessageBox.Show("Папка с таким именем уже существует", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            Directory.Move(Path.Combine(ActiveList.addressText.Text, ActiveList.SelectedItem.ToString()), newPath);
                        }
                    }
                    ActiveList.UpdateContent();
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            selectedFiles.Clear();
            foreach(var file in ActiveList.SelectedItems)
            {
                selectedFiles.Add(ActiveList.addressText.Text + file.ToString());
            }
        }

        private async void buttonPastle_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            string pathDirectory = ActiveList.addressText.Text;
            foreach (var f in ActiveList.SelectedItems)
            {
                files.Add(f.ToString());
            }
            await Copy();
        }
        private async Task DirectoryCopy(string from, string to)
        {
            string newFolder = to + Path.GetFileName(from);
            DirectoryInfo dir = new DirectoryInfo(from);
            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(newFolder);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(newFolder, file.Name), true);
            }
            foreach (DirectoryInfo subdir in dirs)
            {
                DirectoryCopy(subdir.FullName, Path.Combine(newFolder , subdir.Name));
            }
        }

        private async void buttonZip_Click(object sender, EventArgs e)
        {
            EnterName formName = new EnterName("Новый архив", this);
            formName.ShowDialog();
            if (formName.result == DialogResult.OK)
            {
                await Compress(formName.textBox1.Text);
            }
        }

        private void buttonUnzip_Click(object sender, EventArgs e)
        {
            
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonNewFolder.PerformClick();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonDelete.PerformClick();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonRename.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonCopy.PerformClick();
        }

        private void featuresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string info = File.Get
            //MessageBox.Show("",Path.Combine(ActiveList.addressText.Text,ActiveList.SelectedItem.ToString())
        }
        private async Task Compress(string name)
        {
            List<string> files = new List<string>();
            string pathDirectory = ActiveList.addressText.Text;
            foreach (var f in ActiveList.SelectedItems)
            {
                files.Add(f.ToString());
            }
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.AlternateEncoding = Encoding.GetEncoding(866);
                foreach (string item in files)
                {
                    string path = pathDirectory + item;
                    if (File.Exists(path))
                    {
                        await Task.Run(() => zip.AddFile(path, ""));
                    }
                    else
                    {

                        await Task.Run(() => zip.AddDirectory(path, Path.GetFileName(path)));
                    }
                }
                zip.Save(Path.Combine(pathDirectory,name) + ".zip");
                ActiveList.UpdateContent();
            }
        }
        async Task Copy()
        {
            foreach (string file in selectedFiles)
            {
                if (File.GetAttributes(file) == FileAttributes.Directory)
                {
                    DirectoryCopy(file, ActiveList.addressText.Text);
                }
                else
                {
                    File.Copy(file, ActiveList.addressText.Text + Path.GetFileName(file), true);
                }
            }
            listFiles1.UpdateContent();
            listFiles2.UpdateContent();
        }

        private async void listFiles1_DragDrop(object sender, DragEventArgs e)
        {
             await DragDropCopy(listFiles1, e);
        }

        private async void listFiles2_DragDrop(object sender, DragEventArgs e)
        {
             await DragDropCopy(listFiles2, e);
        }
        async Task DragDropCopy(ListFiles list, DragEventArgs e)
        {
            foreach (string f in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (File.Exists(f))
                {
                    File.Copy(f, list.addressText.Text + Path.GetFileName(f), true);
                }
                else
                {
                    await DirectoryCopy(f, list.addressText.Text);
                }
                list.UpdateContent();
            }
        }
    }
}
