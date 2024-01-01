using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public interface IPersistence {
  public const string MAP_DATA_EXTENSION = ".mapdata";
  private const string MAPS_FOLDER = "/Maps";

  private string dataPath {
    get {
      return !Application.isEditor || DEBUG.alwaysSaveToPersistentPath ? Application.persistentDataPath : Application.dataPath;
    }
  }
  private string dataPathForGrids {
    get { return $"{dataPath}{MAPS_FOLDER}"; }
  }

  public void SaveMapData(MapData data) {
    WriteToFile<MapData>(dataPathForGrids, data, MAP_DATA_EXTENSION);
  }
  public MapData LoadMapData(string fileName, string subdirectory = null) {
    var path = subdirectory != null ? $"{dataPathForGrids}{subdirectory}" : dataPathForGrids;
    MapData data = ReadFromFile<MapData>(path, fileName, MAP_DATA_EXTENSION);
    return data;
  }

  //read/write
  private void WriteToFile<T>(string path, T data, string extension) where T : SaveData {
    string fullPath = Path.Combine(path, $"{data.fileName}{extension}");
    try {
      Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
      string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

      using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
        using (StreamWriter writer = new StreamWriter(stream)) {
          writer.Write(dataToStore);
        }
      }
    } catch (System.Exception e) {
      Debug.LogError("Error trying to save data to file: " + fullPath + "\n" + e);
    }
  }
  private T ReadFromFile<T>(string path, string id, string extension) where T : SaveData {
    string fullPath = Path.Combine(path, $"{id}{extension}");
    T loadedData = null;

    if (File.Exists(fullPath)) {
      try {
        string dataToLoad = "";
        using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
          using (StreamReader reader = new StreamReader(stream)) {
            dataToLoad = reader.ReadToEnd();
          }
        }

        loadedData = JsonConvert.DeserializeObject<T>(dataToLoad);
      } catch (System.Exception e) {
        Debug.LogError("Error trying to load data from file: " + fullPath + "\n" + e);
      }
    }
    return loadedData;
  }
}
