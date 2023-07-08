using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData : SaveData {
  public int width;
  public int height;
  public List<TileData> tiles;
  public List<UnitData> units;

  public GridData() : base() {
    SetDefaults();
  }
  public GridData(string fileName) : base(fileName) {
    SetDefaults();
  }

  private void SetDefaults() {
    width = 6;
    height = 6;
    tiles = new List<TileData>();
    units = new List<UnitData>();
  }
}
