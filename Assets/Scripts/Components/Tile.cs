using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour {
  [SerializeField] private TileRenderer tileRenderer;
  [SerializeField] private TileRenderer wallRenderer;
  [SerializeField] private Digit digit;

  public Grid grid;
  public TileData data;
  public TileStatus status {
    get { return _status; }
    set {
      bool updateWalls = (_status == TileStatus.Wall || value == TileStatus.Wall) && _status != value;
      _status = value;
      switch (_status) {
        case TileStatus.Confirmed: digitDisplayMode = DigitDisplayMode.Confirmed; break;
        case TileStatus.Wall: digitDisplayMode = DigitDisplayMode.Wall; break;
        default: digitDisplayMode = DigitDisplayMode.Default; break;
      }

      if (updateWalls) {
        RenderWall();
        var neighbors = GetNeighbors();
        neighbors.ForEach((Tile t) => {
          if (t.status == TileStatus.Wall) t.RenderWall();
        });
      }
    }
  }
  public Point pos { get { return data.pos; } set { data.pos = value; } }
  public Vector2 center { get { return new Vector3(pos.x, pos.y); } }
  public int solutionDigit {
    get { return data.solution; }
    set { data.solution = value; digit.UpdateDigit(); }
  }
  public int currentDigit {
    get { return _currentDigit; }
    set { _currentDigit = value; digitDisplayMode = DigitDisplayMode.Default; }
  }
  public DigitDisplayMode digitDisplayMode { get { return digit.displayMode; } set { digit.displayMode = value; } }
  private int _currentDigit;
  [SerializeField]
  private TileStatus _status;

  //commands
  public void Evaluate(bool allowConfirmation = false) {
    if (status == TileStatus.Confirmed) return;

    if (currentDigit != 0 && currentDigit != solutionDigit) {
      status = TileStatus.Wall;
    } else if (allowConfirmation && currentDigit != 0 && currentDigit == solutionDigit) {
      status = TileStatus.Confirmed;
    } else {
      status = TileStatus.Undecided;
    }
  }
  public void DamageWall(int value) {
    if (status != TileStatus.Wall) {
      Debug.Log("Tried to damage a wall, but that tile isn't a wall");
      return;
    }

    var newVal = currentDigit - value;
    currentDigit = Mathf.Max(0, newVal);
    Evaluate(true);
  }
  public void Load(TileData data, Grid grid) {
    this.data = data;
    this.grid = grid;
    currentDigit = data.given;
    Snap();

    if (data.type == TileType.Cliff) {
      Destroy(digit.gameObject);
      Destroy(wallRenderer.gameObject);
    } else {
      Evaluate(true);
    }
  }
  public void RenderTile() {
    tileRenderer.Render();
  }
  public void RenderWall() {
    if (wallRenderer) wallRenderer.Render();
  }
  void Snap() {
    transform.localPosition = center;
    transform.localScale = new Vector3(1, 1, 1);
  }

  //queries
  public TileData GatherData() {
    if (data.type == TileType.Cliff) return null;
    data.given = currentDigit;
    return data;
  }
  public List<Tile> GetNeighbors() {
    List<Point> pointsToCheck = new List<Point>() {
       new Point(pos.x - 1, pos.y - 1),
       new Point(pos.x - 1, pos.y),
       new Point(pos.x - 1, pos.y + 1),
       new Point(pos.x, pos.y - 1),
       new Point(pos.x, pos.y + 1),
       new Point(pos.x + 1, pos.y - 1),
       new Point(pos.x + 1, pos.y),
       new Point(pos.x + 1, pos.y + 1)
    };

    List<Tile> neighbors = new List<Tile>();
    foreach (Point p in pointsToCheck) {
      if (grid.tiles.ContainsKey(p)) neighbors.Add(grid.tiles[p]);
    }

    return neighbors;
  }
  public bool IsWalkable() {
    return status != TileStatus.Wall;
  }
}
