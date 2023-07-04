using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class Tile : MonoBehaviour {
  public enum DisplayMode { Current, Solution, Faded }

  [SerializeField] Material cliffMat;
  [SerializeField] Material groundMat;
  [SerializeField] Color textColorCurrent;
  [SerializeField] Color textColorSolution;
  [SerializeField] Color textColorFaded;

  private TileData data;
  private Grid grid;
  private TextMeshPro digitText;

  public TileType type { get { return data.type; } }
  public DisplayMode displayMode { get { return _displayMode; } set { _displayMode = value; UpdateDigit(); } }
  public Point pos { get { return data.pos; } set { data.pos = value; } }
  public Vector2 center { get { return new Vector3(pos.x, pos.y); } }
  public int solutionDigit { get { return data.solution; } set { data.solution = value; UpdateDigit(); } }
  public int currentDigit { get { return _currentDigit; } set { _currentDigit = value; UpdateDigit(); } }

  [SerializeField] private int _currentDigit;
  [SerializeField] private DisplayMode _displayMode;

  void Snap() {
    transform.localPosition = center;
    transform.localScale = new Vector3(1, 1, 1);
  }
  public void Load(TileData data, Grid grid) {
    this.data = data;
    this.grid = grid;
    currentDigit = data.given;
    Snap();

    MeshRenderer mRenderer = GetComponent<MeshRenderer>();
    digitText = GetComponentInChildren<TextMeshPro>();
    switch (type) {
      case TileType.Cliff:
        mRenderer.sharedMaterial = cliffMat;
        if (digitText) Destroy(digitText.gameObject);
        break;
      case TileType.Ground:
        mRenderer.sharedMaterial = groundMat;
        UpdateDigit();
        break;
    }
  }
  public TileData GatherData() {
    data.given = currentDigit;
    return data;
  }
  public void UpdateDigit() {
    if (!digitText) return;

    var target = currentDigit;
    var color = textColorCurrent;
    switch (displayMode) {
      case DisplayMode.Solution:
        target = solutionDigit;
        color = textColorSolution;
        break;
      case DisplayMode.Faded:
        color = textColorFaded;
        break;
    }

    digitText.text = target > 0 ? target.ToString() : "";
    digitText.color = color;
  }
  public void SetUVs() {
    Point pWest = new Point(pos.x - 1, pos.y);
    Point pSouth = new Point(pos.x, pos.y - 1);
    Point pEast = new Point(pos.x + 1, pos.y);
    Point pNorth = new Point(pos.x, pos.y + 1);
    //investigate surroundings (-1 = void, 0 = same type, 1 = different type)
    int west = !grid.tiles.ContainsKey(pWest) ? -1 : grid.tiles[pWest].type == type ? 0 : 1;
    int south = !grid.tiles.ContainsKey(pSouth) ? -1 : grid.tiles[pSouth].type == type ? 0 : 1;
    int east = !grid.tiles.ContainsKey(pEast) ? -1 : grid.tiles[pEast].type == type ? 0 : 1;
    int north = !grid.tiles.ContainsKey(pNorth) ? -1 : grid.tiles[pNorth].type == type ? 0 : 1;

    Vector3 coordinates;
    switch (type) {
      case TileType.Cliff: coordinates = CliffCoords(west, south, east, north); break;
      case TileType.Ground: coordinates = GroundCoords(west, south, east, north); break;
      default: coordinates = Vector2.zero; break;
    }

    MeshFilter mFilter = GetComponent<MeshFilter>();
    List<Vector2> uvs = new List<Vector2>(mFilter.mesh.uv);
    float t = GridParams.TexScale;
    uvs[0] = new Vector2(coordinates.x * t, coordinates.y * t);
    uvs[1] = new Vector2(coordinates.x * t + t, coordinates.y * t);
    uvs[2] = new Vector2(coordinates.x * t, coordinates.y * t + t);
    uvs[3] = new Vector2(coordinates.x * t + t, coordinates.y * t + t);

    Mesh mesh = mFilter.mesh;
    mesh.uv = uvs.ToArray();
  }
  Vector2 CliffCoords(int west, int south, int east, int north) {
    //left edge
    if (west < 0) {
      if (east == 0 && north == 0) return new Vector2(0, 0);
      if (east == 0 && south == 0) return new Vector2(0, 6);
      if (north == 0 && south == 0) return new Vector2(0, 1);
      return new Vector2(0, 1);
    }
    //right edge
    if (east < 0) {
      if (west == 0 && north == 0) return new Vector2(1, 0);
      if (west == 0 && south == 0) return new Vector2(1, 6);
      if (north == 0 && south == 0) return new Vector2(1, 1);
      return new Vector2(1, 1);
    }
    //bottom edge
    if (south < 0) {
      if (east == 0 && west == 0) return new Vector2(0, 7);
      return new Vector2(0, 7);
    }
    //top edge
    if (north < 0) {
      if (east == 0 && west == 0) return new Vector2(5, 1);
      return new Vector2(5, 1);
    }

    return Vector2.zero;
  }
  Vector2 GroundCoords(int west, int south, int east, int north) {
    //bottom left corner
    if (west > 0 && south > 0) return new Vector2(0, 0);
    //bottom right corner
    if (east > 0 && south > 0) return new Vector2(7, 0);
    //top left corner
    if (west > 0 && north > 0) return new Vector2(0, 7);
    //top right corner
    if (east > 0 && north > 0) return new Vector2(7, 7);

    //left edge
    if (west > 0 && north == 0 && south == 0) return new Vector2(0, 1);
    //bottom edge
    if (south > 0 && east == 0 && west == 0) return new Vector2(1, 0);
    //right edge
    if (east > 0 && north == 0 && south == 0) return new Vector2(7, 1);
    //top edge
    if (north > 0 && east == 0 && west == 0) return new Vector2(1, 7);

    //center
    return new Vector2(1, 1);
  }
}
