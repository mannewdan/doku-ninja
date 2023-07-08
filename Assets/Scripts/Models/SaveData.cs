using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
  public string id;
  public string fileName { get { return string.IsNullOrEmpty(_fileName) ? id : _fileName; } }
  private string _fileName;

  public SaveData() { id = System.Guid.NewGuid().ToString(); }
  public SaveData(string fileName) {
    id = System.Guid.NewGuid().ToString();
    _fileName = fileName;
  }
}
