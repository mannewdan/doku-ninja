using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData {
  public string id;
  public int width;
  public int height;
  public List<TileData> tiles;
  public List<UnitData> units;

  public GridData() {
    id = System.Guid.NewGuid().ToString() + IPersistence.GRID_DATA_EXTENSION;
    width = 6;
    height = 6;
    tiles = new List<TileData>();
    units = new List<UnitData>();
  }
}
