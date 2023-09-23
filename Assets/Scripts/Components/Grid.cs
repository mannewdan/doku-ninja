using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Grid : MonoBehaviour {
  [SerializeField] GameObject tilePrefab;

  public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
  public int width;
  public int height;

  public class Conflict {
    public Tile tile;
    public bool row, column, box;
    public Conflict(Tile tile, bool row, bool column, bool box) {
      this.tile = tile;
      this.row = row;
      this.column = column;
      this.box = box;
    }
  }

  //commands
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
    foreach (TileData t in data.tiles) {
      if (tiles.ContainsKey(t.pos)) {
        tiles[t.pos].RenderWall();
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
    mapData.width = width;
    mapData.height = height;
    mapData.tiles.Clear();
    foreach (Tile t in tiles.Values) {
      var d = t.GatherData();
      if (d != null) mapData.tiles.Add(d);
    }
  }
  public bool ValidateBoard(Tile lastTileEdited) {
    bool allSolved = true;
    List<Conflict> conflicts = new List<Conflict>();
    bool lastTileRow = false, lastTileColumn = false, lastTileBox = false;
    foreach (KeyValuePair<Point, Tile> tileEntry in tiles) {
      Tile tile = tileEntry.Value;
      if (tile.currentDigit != tile.solutionDigit) allSolved = false;
      if (tile.pos == lastTileEdited.pos) continue;
      if (tile.currentDigit != lastTileEdited.currentDigit) continue;

      bool row = false, column = false, box = false;
      if (tile.data.pos.y == lastTileEdited.data.pos.y) { row = true; lastTileRow = true; }
      if (tile.data.pos.x == lastTileEdited.data.pos.x) { column = true; lastTileColumn = true; }
      if (BoxNumber(tile.data.pos.x, tile.data.pos.y) == BoxNumber(lastTileEdited.pos.x, lastTileEdited.pos.y)) {
        box = true; lastTileBox = true;
      }

      if (row || column || box) {
        conflicts.Add(new Conflict(tile, row, column, box));
      }
    }
    if (lastTileRow || lastTileColumn || lastTileBox) {
      conflicts.Add(new Conflict(lastTileEdited, lastTileRow, lastTileColumn, lastTileBox));
    }

    foreach (Conflict c in conflicts) {
      c.tile.Evaluate();
    }

    return allSolved;
  }

  //construction
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

  //queries
  public bool InBounds(Point p) {
    if (p.x < 0 || p.x > width - 1) return false;
    if (p.y < 0 || p.y > height - 1) return false;
    return true;
  }
  public int BoxNumber(int x, int y) {
    int boxWidth = width < 6 ? 2 : 3;
    int boxHeight = width < 9 ? 2 : 3;
    int h = x / boxWidth;
    int v = y / boxHeight;
    return v * boxHeight + h + 1;
  }
}

