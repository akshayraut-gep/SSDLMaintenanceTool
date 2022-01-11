using SSDLMaintenanceTool.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSDLMaintenanceTool
{
    public partial class FileCorruptValidator : Form
    {
        public string FilePath { get; set; }
        private readonly List<string> ExtensionList = new List<string>
        {
            ".txt", ".csv", ".xls", ".xlsx", ".xlsb"
        };
        public FileCorruptValidator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialogResult = this.openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                FilePath = this.openFileDialog1.FileName;
                textBox1.Text = FilePath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var fileExtension = "";
                if (FilePath.HasContent())
                {
                    fileExtension = Path.GetExtension(FilePath).ToLower();
                }

                if (!ExtensionList.Contains(fileExtension.ToLower()))
                {
                    throw new Exception("File may be corrupt because invalid file extension found");
                }

                Task.Factory.StartNew(() =>
                {
                    using (FileStream fileStream = File.OpenRead(FilePath))
                    {
                        if (fileExtension == ".csv" || fileExtension == ".txt")
                        {
                            var csvHelper = new CSVHelper();
                            Encoding encoding = textBox2.Text.HasContent() ? csvHelper.GetEncoding(textBox2.Text) : System.Text.Encoding.UTF8;

                            if (encoding == Encoding.UTF8)
                            {
                                var isFileCorrupt = !csvHelper.IsFileValid(fileStream, fileStream.Length);

                                if (isFileCorrupt)
                                {
                                    throw new IOException("File may be corrupt because non-readable characters were found");
                                }
                            }
                        }
                        else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            var excelHelper = new ExcelHelper();
                            var isFileCorrupt = !(excelHelper.IsFileValid(fileStream, fileExtension).Result);

                            if (!isFileCorrupt)
                            {
                                MessageBox.Show(this, "File is valid");
                                //throw new IOException("File may be corrupt because non-readable characters were found");
                            }
                            else
                            {
                                MessageBox.Show(this, "File may be corrupt because non-readable characters were found");
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            finally
            {

            }
        }
    }
}
