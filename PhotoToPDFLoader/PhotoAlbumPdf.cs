using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace WastechPhotoPDFLoader
{
  static class PhotoAlbumPdf
  {
    // requires collection files at tempStorage directory (/temp)
    // optional argument isLandscape defaulted to true
    public static void GeneratePhotoAlbumPdf(string title, string tempStorage, Photo[] files, string filename, bool isLandscape = true)
    {
      tempStorage += "temp\\";

      var document = new PdfDocument();
      var font = new XFont("Verdana", 20, XFontStyle.Bold);

      PdfPage page = document.AddPage();
      page.Orientation = (isLandscape) ? PageOrientation.Landscape : PageOrientation.Portrait;
      XGraphics gfx = XGraphics.FromPdfPage(page);
      gfx.DrawString(title, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

      // loop for files
      foreach (Photo file in files)
      {
        page = document.AddPage();
        page.Orientation = (isLandscape) ? PageOrientation.Landscape : PageOrientation.Portrait;
        gfx = XGraphics.FromPdfPage(page);
        // image disposed off automatically once leaving using block
        using (XImage image = XImage.FromFile(tempStorage + file.FileName))
        {
          // largest photo dimensions allowed (width - 100 x height - 100)
          double width = image.PixelWidth;
          double height = image.PixelHeight;
          if (width > page.Width - 100)
          {
            double ratio = (page.Width - 100) / width;
            width *= ratio;
            height *= ratio;
          }

          if (height > page.Height - 100)
          {
            double ratio = (page.Height - 100) / height;
            width *= ratio;
            height *= ratio;
          }
          gfx.DrawImage(image, (page.Width - width) / 2, (page.Height - height) / 2, width, height);
        }
      }

      document.Save(filename);
      //PhotoAlbumPdf.DeleteTempCollection(tempStorage, files);
    }

    // delete all files from tempStorage
    /*private static void DeleteTempCollection(string tempStorage, Photo[] files)
    {
      foreach (Photo file in files)
      {
        File.Delete(tempStorage + file.FileName);
      }
    }*/

    // not required method
    // return true if folder at folderPath has permissions above read-only
    /*private static bool HasWriteAccess(string folderPath)
    {
      try
      {
        System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
        return true;
      }
      catch (UnauthorizedAccessException)
      {
        return false;
      }
    }*/
  }
}
