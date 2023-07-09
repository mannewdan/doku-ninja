using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData : SaveData {
  public int width;
  public int height;
  public Point spawn;
  public List<TileData> tiles;
  public List<EnemyData> enemies;

  public MapData() : base() {
    SetDefaults();
  }
  public MapData(string fileName) : base(fileName) {
    SetDefaults();
  }

  private void SetDefaults() {
    spawn = new Point(0, 0);
    width = 6;
    height = 6;
    tiles = new List<TileData>();
    enemies = new List<EnemyData>();
  }
}
