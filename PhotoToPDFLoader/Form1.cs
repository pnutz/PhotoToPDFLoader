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
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace WastechPhotoPDFLoader
{
  public partial class Form1 : Form
  {
    private string folderName;
    private Config config;
    private PhotoCollection collection;

    public Form1()
    {
      InitializeComponent();
      imageList1 = new ImageList();
      imageList1.ImageSize = new Size(255, 255);
      imageList1.TransparentColor = Color.White;

      config = new Config();
      collection = new PhotoCollection(config["photo-source"]);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      Photo[] photos = collection.Collection;
      // imageList1 is for rendering pictures - issue is it has a set size
      // use new Bitmap() to resize to aspect ratio before adding to imageList1
      /*
      int thumbnailSize = 255;
      Image image = Image.FromFile(photo.Directory + photo.FileName);
      double width = image.PixelWidth;
      double height = image.PixelHeight;
      if (width > height)
      {
        width = thumbnailSize;
        height = height * thumbnailSize / width;
      }

      if (height > width)
      {
        height = thumbnailSize;
        width = width * thumbnailSize / height;
      }*/
      // check for integer overflow

      foreach (Photo photo in photos)
      {
        imageList1.Images.Add(Image.FromFile(photo.Directory + photo.FileName));
      }
      if (imageList1.Images.Empty != true)
      {
        pictureBox1.Image = imageList1.Images[0];
        pictureBox1.Height = imageList1.Images[0].Height;
        pictureBox1.Width = imageList1.Images[0].Width;
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult result = folderBrowserDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            folderName = folderBrowserDialog1.SelectedPath;
            label1.Text += " " + folderName;
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
      saveFileDialog1.OverwritePrompt = true;
      saveFileDialog1.InitialDirectory = "C:\\Users\\ceng\\Desktop";
      saveFileDialog1.FileName = "hihih test";

      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Test Document";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
        gfx.DrawString("Test Title", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height - 20), XStringFormats.Center);

        // make it landscape
        //string filename = Path.GetDirectoryName(saveFileDialog1.FileName);

        document.Save(saveFileDialog1.FileName);
      }
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {

    }
  }
}
