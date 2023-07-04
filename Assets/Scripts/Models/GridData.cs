using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : ScriptableObject {
  public string id;
  public int width;
  public int height;
  public List<TileData> tiles;

  public GridData() {
    id = "default";
    width = 6;
    height = 6;
    tiles = new List<TileData>();
  }
}
