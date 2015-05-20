using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastechPhotoPDFLoader
{
    static class Program
    {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
        try
        {
          var config = new Config();
          var collection = new PhotoCollection(config["photo-source"]);
          collection.AddAllToSelectedCollection();
          //collection.CopySelectedCollection(config["temp-storage"] + "temp\\");
          //PhotoAlbumPdf.GeneratePhotoAlbumPdf("test doc", config["temp-storage"], collection.SelectedCollection, config["pdf-destination"] + config["pdf-format"]);
          //collectionDeleteSelectedCollection();
          //collection.RefreshPhotoCollection();
        }
        catch (Exception e)
        {

        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
    }
}
