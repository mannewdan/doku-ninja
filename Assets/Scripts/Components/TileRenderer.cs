using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour {
  [SerializeField] Material cliffMat;
  [SerializeField] Material groundMat;

  private Tile owner;
  public TileType type { get { return owner.data.type; } }
  public Point pos { get { return owner.pos; } }
  public Grid grid { get { return owner.grid; } }

  protected void Awake() {
    owner = GetComponent<Tile>();
  }
  public void SetMaterialAndUVs() {
    MeshRenderer mRenderer = GetComponent<MeshRenderer>();
    switch (type) {
      case TileType.Cliff:
        mRenderer.sharedMaterial = cliffMat;
        break;
      case TileType.Ground:
        mRenderer.sharedMaterial = groundMat;
        break;
    }

    Point pWest = new Point(pos.x - 1, pos.y);
    Point pSouth = new Point(pos.x, pos.y - 1);
    Point pEast = new Point(pos.x + 1, pos.y);
    Point pNorth = new Point(pos.x, pos.y + 1);
    //investigate surroundings (-1 = void, 0 = same type, 1 = different type)
    int west = !grid.tiles.ContainsKey(pWest) ? -1 : grid.tiles[pWest].data.type == type ? 0 : 1;
    int south = !grid.tiles.ContainsKey(pSouth) ? -1 : grid.tiles[pSouth].data.type == type ? 0 : 1;
    int east = !grid.tiles.ContainsKey(pEast) ? -1 : grid.tiles[pEast].data.type == type ? 0 : 1;
    int north = !grid.tiles.ContainsKey(pNorth) ? -1 : grid.tiles[pNorth].data.type == type ? 0 : 1;

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
