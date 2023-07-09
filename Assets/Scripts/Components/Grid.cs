using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
  [SerializeField] GameObject tilePrefab;

  public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
  private int width;
  private int height;

  public void Load(MapData data) {
    width = data.width;
    height = data.height;
    ClearMap();
    BuildMap();
    foreach (TileData t in data.tiles) {
      if (tiles.ContainsKey(t.pos)) {
        tiles[t.pos].Load(t, this);
      } else {
        Debug.Log("Failed to load tile at pos: " + t.pos + " because a tile does not exist at that position");
      }
    }
  }
  public void BuildMap() {
    GenerateGrid();
    GenerateBorder();

    foreach (KeyValuePair<Point, Tile> tile in tiles) {
      tile.Value.RenderTile();
    }

    Lines lines = transform.parent.GetComponentInChildren<Lines>();
    if (lines) lines.Initialize(width, height);
  }
  public void ClearMap() {
    var keys = new List<Point>(tiles.Keys);
    for (int i = 0; i < keys.Count; i++) {
      Destroy(tiles[keys[i]].gameObject);
    }
    tiles.Clear();

    Lines lines = transform.parent.GetComponentInChildren<Lines>();
    if (lines) lines.Clear();
  }
  public void GatherData(ref MapData mapData) {
    if (mapData == null) mapData = new MapData();

    mapData.width = width;
    mapData.height = height;
    mapData.tiles.Clear();
    foreach (Tile t in tiles.Values) {
      var d = t.GatherData();
      if (d != null) mapData.tiles.Add(d);
    }
  }

  void GenerateGrid() {
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        NewTile(new TileData() { pos = new Point(x, y), type = TileType.Ground });
      }
    }
  }
  void GenerateBorder() {
    for (int x = -1; x < width + 1; x++) {
      for (int y = -1; y < height + 1; y++) {
        if (!(x == -1 || x == width || y == -1 || y == height)) continue;

        NewTile(new TileData() { pos = new Point(x, y), type = TileType.Cliff });
      }
    }
  }

  Tile NewTile(TileData data) {
    GameObject newTile = Instantiate(tilePrefab) as GameObject;
    newTile.transform.SetParent(transform);
    Tile t = newTile.GetComponent<Tile>();
    t.Load(data, this);
    tiles.Add(t.pos, t);
    return t;
  }
}

