using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastechPhotoPDFLoader
{
  class Photo
  {
    private int index;
    public int Index
    {
      get { return index; }
      set { index = value; }
    }

    private string directory;
    public string Directory
    {
      get { return directory; }
      set { Directory = value; }
    }

    private string filename;
    public string FileName
    {
      get { return filename; }
      set { filename = value; }
    }

    public Photo(int index, string directory, string filename)
    {
      this.index = index;
      this.directory = directory;
      this.filename = filename;
    }
  }
}
