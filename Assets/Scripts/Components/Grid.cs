using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
  [SerializeField] GameObject tilePrefab;

  public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
  private int width;
  private int height;

  public void Initialize(int width, int height) {
    ClearMap();
    this.width = width;
    this.height = height;
    BuildMap();
  }
  public void Load(GridData data) {
    ClearMap();
    width = data.width;
    height = data.height;
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
    GenerateGrid(width, height);
    GenerateBorder(width, height);

    foreach (KeyValuePair<Point, Tile> tile in tiles) {
      tile.Value.SetUVs();
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
  public GridData GatherData() {
    GridData data = new GridData();
    data.width = width;
    data.height = height;
    data.tiles.Clear();
    foreach (Tile t in tiles.Values) {
      if (t.type == TileType.Cliff) continue;
      data.tiles.Add(t.GatherData());
    }
    return data;
  }

  void GenerateGrid(int width, int height) {
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        NewTile(new TileData() { pos = new Point(x, y), type = TileType.Ground });
      }
    }
  }
  void GenerateBorder(int width, int height) {
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

