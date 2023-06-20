using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
  [SerializeField] GameObject tilePrefab;
  [SerializeField] int width;
  [SerializeField] int height;

  public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

  public void Load(GridData data) {
    for (int i = 0; i < data.tiles.Count; i++) {
      NewTile(data.tiles[i]);
    }
  }
  public void Start() {
    for (int x = 0; x < width; x++) {
      for (int y = 0; y < height; y++) {
        NewTile(new TileData() { pos = new Point(x, y) });
      }
    }
  }

  Tile NewTile(TileData data) {
    GameObject newTile = Instantiate(tilePrefab) as GameObject;
    newTile.transform.parent = transform;
    Tile t = newTile.GetComponent<Tile>();
    t.Load(data);
    tiles.Add(t.pos, t);
    return t;
  }
}

