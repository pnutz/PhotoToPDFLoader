using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WastechPhotoPDFLoader
{
  public class Config
  {
    private static string configDirectory = "\\WastechPhotoPDFLoader\\";
    private static string configFile = "config.cfg";

    // key-value configuration store
    private IDictionary<string, string> configDict = new Dictionary<string, string> {
      { "photo-source", Environment.GetLogicalDrives()[Environment.GetLogicalDrives().Length - 1] },
      { "photo-destination", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\{date}\\" },
      { "pdf-destination", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" },
      { "temp-storage", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + configDirectory },
      { "station-code", "CRRP" },
      { "pdf-format", "{station-code}_{date}.pdf" },
      { "date-format", "MM-dd-yyyy" }
    };
    public string this[string key]
    {
      get
      {
        string result = configDict[key];
        if (String.Compare(key, "pdf-format") == 0)
        {
          foreach (KeyValuePair<string, string> kvp in configDict)
          {
            // ignore pdf-format and date-format, look for match in pdf-format
            if (String.Compare(kvp.Key, "pdf-format") != 0 && String.Compare(kvp.Key, "date-format") != 0 && result.IndexOf("{" + kvp.Key + "}") != -1)
            {
              result = result.Replace("{" + kvp.Key + "}", kvp.Value);
            }
          }
        }
        result = result.Replace("{date}", DateTime.Today.ToString(configDict["date-format"]));
        return result;
      }
      set
      {
        configDict[key] = value;
      }
    }
    
    public Config()
    {
      SetConfigLocation();
      SetConfigFile();
    }

    private void SetConfigLocation()
    {
      if (Directory.Exists(configDict["temp-storage"]))
      {
        return;
      }

      DirectoryInfo di = Directory.CreateDirectory(configDict["temp-storage"]);
    }

    private void SetConfigFile()
    {
      // check if config file doesn't exist
      if (File.Exists(configDict["temp-storage"] + configFile))
      {
        var keysFound = new List<string>();
        bool addToList = true;
        string key;
        string value;
        string line;
        StreamReader reader = File.OpenText(configDict["temp-storage"] + configFile);
        while ((line = reader.ReadLine()) != null)
        {
          key = "";
          value = "";
          foreach (KeyValuePair<string, string> kvp in configDict)
          {
            // found first configuration match in file
            if (line.IndexOf(kvp.Key) == 0 && line.IndexOf("=") == kvp.Key.Length)
            {
              if (addToList)
              {
                keysFound.Add(kvp.Key);
              }

              string tempValue = line.Substring(kvp.Key.Length + 1);
              // if a value is blank, delete keysFound and stop adding to it
              if (String.IsNullOrEmpty(tempValue))
              {
                addToList = false;
                keysFound.Clear();
              }
              // value differs from key-value defaults
              else if (String.Compare(tempValue, kvp.Value) != 0)
              {
                key = kvp.Key;
                value = tempValue;
              }
              break;
            }
          }
            
          // change local dict to match config file if not default
          if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value))
          {
            configDict[key] = value;
          }
        }
        reader.Close();

        // add key defaults to file that were not found
        if (keysFound.Count > 0)
        {
          StreamWriter writer = File.AppendText(configDict["temp-storage"] + configFile);
          foreach (KeyValuePair<string, string> kvp in configDict)
          {
            if (!keysFound.Contains(kvp.Key))
            {
              writer.WriteLine("{0}={1}", kvp.Key, kvp.Value);
            }
          }
          writer.Close();
        }
        // if blank value was found or file was blank, recreate config file
        else
        {
          CreateConfigFile();
        }
      }
      else
      {
        CreateConfigFile();
      }
    }

    private void CreateConfigFile()
    {
      StreamWriter writer = File.CreateText(configDict["temp-storage"] + configFile);
      writer.WriteLine("// Format by new-line: key=value");
      writer.WriteLine("// {date} will be replaced by the current date in date-format");
      writer.WriteLine("// date-format key-value follows custom date and time format string convention: https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx");
      writer.WriteLine("// Contact the developer to add additional configurations to the application");
      foreach (KeyValuePair<string, string> kvp in configDict)
      {
        writer.WriteLine("{0}={1}", kvp.Key, kvp.Value);
      }
      writer.Close();
    }
  }
}
