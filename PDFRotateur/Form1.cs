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
using iTextSharp.text.pdf;

namespace PDFRotateur
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button6_Click(sender, e);
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        string rotationOutputFilePath = Application.StartupPath;
        string messageBoxText = "";
        string messageBoxTitle = "";
        MemoryStream outputFileStream;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog1.FileName;
                    textBox1.Text = filePath;
                    pdfViewer1.Document = PdfiumViewer.PdfDocument.Load(filePath);
                }
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() != "" && comboBox1.Text.Trim() != "")
                {
                    if (outputFileStream != null) outputFileStream.Close();
                    string timestamp = GetTimestamp(DateTime.Now);
                    string outputFilePath = Path.Combine(rotationOutputFilePath, timestamp + ".pdf");

                    PdfReader reader = new PdfReader(textBox1.Text);
                    int pagesCount = reader.NumberOfPages;

                    for (int n = 1; n <= pagesCount; n++)
                    {
                        PdfDictionary page = reader.GetPageN(n);
                        PdfNumber rotate = page.GetAsNumber(PdfName.ROTATE);
                        int rotation = Convert.ToInt32(comboBox1.Text);

                        page.Put(PdfName.ROTATE, new PdfNumber(rotation));
                    }

                    FileStream fs = File.OpenWrite(outputFilePath);
                    PdfStamper stamper = new PdfStamper(reader, fs);
                    stamper.Close();
                    reader.Close();

                    outputFileStream = new MemoryStream(File.ReadAllBytes(outputFilePath));
                    pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(outputFileStream);
                    File.Delete(outputFilePath);

                    //pdfViewer2.Document = null;
                    //pdfViewer2.Document = PdfiumViewer.PdfDocument.Load(textBox1.Text);
                    //int rotation = Convert.ToInt32(comboBox1.Text);
                    //for (int i = 0; i < pdfViewer1.Document.PageCount; i++)
                    //{
                    //    switch (rotation)
                    //    {
                    //        case 0:
                    //            pdfViewer2.Document.RotatePage(i, PdfiumViewer.PdfRotation.Rotate0);
                    //            break;
                    //        case 90:
                    //            pdfViewer2.Document.RotatePage(i, PdfiumViewer.PdfRotation.Rotate90);
                    //            break;
                    //        case 180:
                    //            pdfViewer2.Document.RotatePage(i, PdfiumViewer.PdfRotation.Rotate180);
                    //            break;
                    //        case 270:
                    //            pdfViewer2.Document.RotatePage(i, PdfiumViewer.PdfRotation.Rotate270);
                    //            break;
                    //    }
                    //}
                }
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName != "")
                {
                    pdfViewer2.Document.Save(saveFileDialog1.FileName);
                    MessageBox.Show(messageBoxText, messageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "Choose a PDF file to rotate";
            button1.Text = "BROWSE";
            label3.Text = "Choose a rotation degree";
            button4.Text = "Rotate";
            button3.Text = "SAVE AS";
            this.Text = "PDF files rotation tool";
            openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog1.Title = "Choose a PDF file to rotate";
            saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog1.Title = "Save the PDF file";
            messageBoxText = "The operation completed successfully";
            messageBoxTitle = "Completed";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = "Choisissez un fichier PDF pour faire pivoter";
            button1.Text = "OUVRIR";
            label3.Text = "Choisissez un degré de rotation";
            button4.Text = "Faire pivoter";
            button3.Text = "ENREGISTRER SOUS";
            this.Text = "Programme de rotation des fichiers PDF";
            openFileDialog1.Filter = "Fichier PDF (*.pdf)|*.pdf";
            openFileDialog1.Title = "Choisir un fichier PDF";
            saveFileDialog1.Filter = "Fichier PDF (*.pdf)|*.pdf";
            saveFileDialog1.Title = "Enregistrer un fichier PDF";
            messageBoxText = "L'opération s'est terminée avec succès";
            messageBoxTitle = "Terminée";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Chọn 1 tập tin PDF để xoay";
            button1.Text = "MỞ";
            label3.Text = "Chọn độ xoay";
            button4.Text = "Xoay";
            button3.Text = "LƯU DƯỚI DẠNG";
            this.Text = "Công cụ xoay tập tin PDF";
            openFileDialog1.Filter = "Tập tin PDF (*.pdf)|*.pdf";
            openFileDialog1.Title = "Chọn 1 tập tin PDF để xoay";
            saveFileDialog1.Filter = "Tập tin PDF (*.pdf)|*.pdf";
            saveFileDialog1.Title = "Lưu tập tin PDF";
            messageBoxText = "Tác vụ hoàn tất";
            messageBoxTitle = "Hoàn thành";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (outputFileStream != null) outputFileStream.Close();
            }
            catch (Exception e_)
            {
                //MessageBox.Show(e_.Message);
            }
        }
    }
}
