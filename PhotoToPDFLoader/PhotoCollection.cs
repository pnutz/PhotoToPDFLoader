using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WastechPhotoPDFLoader
{
  // collection of Photos
  class PhotoCollection
  {
    private static string[] extensions = { ".jpg", ".tif", ".png", ".gif", ".bmp", ".jpeg" };

    private string directory;

    private List<Photo> collection = new List<Photo>();
    public Photo[] Collection
    {
      get { return collection.ToArray(); }
    }

    // default empty
    private List<Photo> selectedCollection = new List<Photo>();
    // returns array of path strings
    public Photo[] SelectedCollection
    {
      get { return selectedCollection.ToArray(); }
    }

    public PhotoCollection(string directory)
    {
      this.directory = directory;
      var dInfo = new DirectoryInfo(directory);
      FileInfo[] imageData = dInfo.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
      int index = 0;
      foreach (FileInfo image in imageData)
      {
        collection.Add(new Photo(index, image.DirectoryName + "\\", image.Name));
        index++;
      }
    }

    // add all of collection to selectedCollection
    public void AddAllToSelectedCollection()
    {
      selectedCollection = new List<Photo>();
      selectedCollection = collection;
    }

    // add index of collection to selectedCollection
    public void AddToSelectedCollection(int index)
    {
      var insertIndex = -1;
      for (int i = 0; i < selectedCollection.Count; i++)
      {
        if (selectedCollection[i].Index > index)
        {
          insertIndex = i;
          break;
        }
      }

      // add to end of list
      if (insertIndex == -1)
      {
        selectedCollection.Insert(selectedCollection.Count, collection[index]);
      }
      // insert into insertIndex of list
      else
      {
        selectedCollection.Insert(insertIndex, collection[index]);
      }
    }

    // remove photo with index from selected collection
    public void RemoveFromSelectedCollection(int index)
    {
      var removeIndex = -1;
      for (int i = 0; i < selectedCollection.Count; i++)
      {
        if (selectedCollection[i].Index == index)
        {
          removeIndex = i;
          break;
        }
      }

      if (removeIndex != -1)
      {
        selectedCollection.RemoveAt(removeIndex);
      }
    }

    // create copy at destination (overwrites if already exists)
    public void CopySelectedCollection(string destination)
    {
      if (!Directory.Exists(destination))
      {
        DirectoryInfo di = Directory.CreateDirectory(destination);
      }

      for (int i = 0; i < selectedCollection.Count; i++)
      {
        File.Copy(selectedCollection[i].Directory + selectedCollection[i].FileName, destination + selectedCollection[i].FileName, true);
      }
    }

    // move to destination (and delete original) files
    public void MoveSelectedCollection(string destination)
    {
      if (!Directory.Exists(destination))
      {
        DirectoryInfo di = Directory.CreateDirectory(destination);
      }

      for (int i = 0; i < selectedCollection.Count; i++)
      {
        File.Move(selectedCollection[i].Directory + selectedCollection[i].FileName, destination + selectedCollection[i].FileName);
      }
    }

    // delete all files from selectedCollection
    private void DeleteSelectedCollection()
    {
      foreach (Photo file in selectedCollection)
      {
        File.Delete(file.Directory + file.FileName);
      }
    }

    // clear selected collection and reset collection
    public void RefreshPhotoCollection()
    {
      selectedCollection = new List<Photo>();
      var dInfo = new DirectoryInfo(directory);
      FileInfo[] imageInfo = dInfo.GetFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
      int index = 0;
      foreach (FileInfo image in imageInfo)
      {
        collection.Add(new Photo(index, image.DirectoryName + "\\", image.Name));
        index++;
      }
    }
  }
}
