using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour {
  static readonly List<byte> eightBitValues = new List<byte> { 0, 1, 2, 4, 5, 8, 9, 10, 16, 17, 18, 20, 21, 32, 33, 34, 36, 37, 40, 41, 42, 64, 65, 66, 68, 69, 72, 73, 74, 80, 81, 82, 84, 85, 128, 130, 132, 136, 138, 144, 146, 148, 160, 162, 164, 168, 170 };
  public enum RenderType { Tile, Wall }

  [SerializeField] Material cliffMat;
  [SerializeField] Material groundMat;
  [SerializeField] Material wallMat;
  [SerializeField] RenderType renderType;
  private Tile owner;
  public TileType type { get { return renderType == RenderType.Wall ? TileType.Wall : owner.data.type; } }
  public Point pos { get { return owner.pos; } }
  public Grid grid { get { return owner.grid; } }

  protected void Awake() {
    owner = GetComponent<Tile>();
    if (!owner) owner = GetComponentInParent<Tile>();
    SetBlank();
  }
  public void SetBlank() {
    MeshFilter mFilter = GetComponent<MeshFilter>();
    List<Vector2> uvs = new List<Vector2>(mFilter.mesh.uv);

    float t = GridParams.TexScale;
    Vector2 coordinates = new Vector2(7, 7);

    uvs[0] = new Vector2(coordinates.x * t, coordinates.y * t);
    uvs[1] = new Vector2(coordinates.x * t + t, coordinates.y * t);
    uvs[2] = new Vector2(coordinates.x * t, coordinates.y * t + t);
    uvs[3] = new Vector2(coordinates.x * t + t, coordinates.y * t + t);

    Mesh mesh = mFilter.mesh;
    mesh.uv = uvs.ToArray();
  }
  public void Render() {
    MeshRenderer mRenderer = GetComponent<MeshRenderer>();
    switch (type) {
      case TileType.Cliff:
        mRenderer.sharedMaterial = cliffMat;
        break;
      case TileType.Ground:
        mRenderer.sharedMaterial = groundMat;
        break;
      case TileType.Wall:
        mRenderer.sharedMaterial = wallMat;
        break;
    }

    Point pWest = new Point(pos.x - 1, pos.y);
    Point pSouth = new Point(pos.x, pos.y - 1);
    Point pEast = new Point(pos.x + 1, pos.y);
    Point pNorth = new Point(pos.x, pos.y + 1);
    Point pSouthwest = new Point(pos.x - 1, pos.y - 1);
    Point pSoutheast = new Point(pos.x + 1, pos.y - 1);
    Point pNorthwest = new Point(pos.x - 1, pos.y + 1);
    Point pNortheast = new Point(pos.x + 1, pos.y + 1);
    //investigate surroundings (-1 = void, 0 = same type, 1 = different type)
    int west = !grid.tiles.ContainsKey(pWest) ? -1 : grid.tiles[pWest].data.type == type ? 0 : 1;
    int south = !grid.tiles.ContainsKey(pSouth) ? -1 : grid.tiles[pSouth].data.type == type ? 0 : 1;
    int east = !grid.tiles.ContainsKey(pEast) ? -1 : grid.tiles[pEast].data.type == type ? 0 : 1;
    int north = !grid.tiles.ContainsKey(pNorth) ? -1 : grid.tiles[pNorth].data.type == type ? 0 : 1;
    Vector3 coordinates;
    switch (type) {
      case TileType.Cliff: coordinates = CliffCoords(west, south, east, north); break;
      case TileType.Ground: coordinates = GroundCoords(west, south, east, north); break;
      case TileType.Wall:
        if (owner.digitStatus != DigitStatus.Wall) { coordinates = new Vector2(7, 7); break; }

        west = !grid.tiles.ContainsKey(pWest) ? -1 : grid.tiles[pWest].digitStatus == DigitStatus.Wall ? 0 : 1;
        south = !grid.tiles.ContainsKey(pSouth) ? -1 : grid.tiles[pSouth].digitStatus == DigitStatus.Wall ? 0 : 1;
        east = !grid.tiles.ContainsKey(pEast) ? -1 : grid.tiles[pEast].digitStatus == DigitStatus.Wall ? 0 : 1;
        north = !grid.tiles.ContainsKey(pNorth) ? -1 : grid.tiles[pNorth].digitStatus == DigitStatus.Wall ? 0 : 1;
        int southwest = !grid.tiles.ContainsKey(pSouthwest) ? -1 : grid.tiles[pSouthwest].digitStatus == DigitStatus.Wall ? 0 : 1;
        int southeast = !grid.tiles.ContainsKey(pSoutheast) ? -1 : grid.tiles[pSoutheast].digitStatus == DigitStatus.Wall ? 0 : 1;
        int northwest = !grid.tiles.ContainsKey(pNorthwest) ? -1 : grid.tiles[pNorthwest].digitStatus == DigitStatus.Wall ? 0 : 1;
        int northeast = !grid.tiles.ContainsKey(pNortheast) ? -1 : grid.tiles[pNortheast].digitStatus == DigitStatus.Wall ? 0 : 1;
        coordinates = WallCoords(west, south, east, north, southwest, southeast, northwest, northeast); break;
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
  Vector2 WallCoords(int w, int s, int e, int n, int sw, int se, int nw, int ne) {
    byte value = 0;
    if (n != 0) value += 128;
    if (ne != 0 && n == 0 && e == 0) value += 64;
    if (e != 0) value += 32;
    if (se != 0 && s == 0 && e == 0) value += 16;
    if (s != 0) value += 8;
    if (sw != 0 && s == 0 && w == 0) value += 4;
    if (w != 0) value += 2;
    if (nw != 0 && n == 0 && w == 0) value += 1;

    int coordIndex = eightBitValues.IndexOf(value);
    if (coordIndex >= 0) {
      return new Vector2(coordIndex % 8, coordIndex / 8);
    } else return new Vector2(7, 7);
  }
}
