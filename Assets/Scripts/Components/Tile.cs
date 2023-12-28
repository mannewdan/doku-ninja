using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class Tile : MonoBehaviour {
  [SerializeField] private TileRenderer tileRenderer;
  [SerializeField] private TileRenderer wallRenderer;
  [SerializeField] private TileEntity tileEntity;
  [SerializeField] private Digit digit;

  [NonSerialized] public Grid grid;
  public TileData data;
  public DigitStatus digitStatus {
    get { return _digitStatus; }
    set {
      bool updateWalls = (_digitStatus == DigitStatus.Wall || value == DigitStatus.Wall) && _digitStatus != value;
      _digitStatus = value;
      switch (_digitStatus) {
        case DigitStatus.Confirmed:
          digitDisplayMode = DigitDisplayMode.Confirmed;
          break;
        case DigitStatus.Wall:
          digitDisplayMode = DigitDisplayMode.Wall;
          break;
        default:
          digitDisplayMode = DigitDisplayMode.Default;
          break;
      }
      if (updateWalls) {
        this.PostNotification(Notifications.MAP_WALL_CHANGED, pos);
        RenderWall();
        var neighbors = GetNeighbors();
        neighbors.ForEach((Tile t) => {
          if (t.digitStatus == DigitStatus.Wall) t.RenderWall();
        });
      }

    }
  }
  public BombStatus bombStatus {
    get { return _bombStatus; }
    set {
      bool wasBomb = HasBomb();
      _bombStatus = value;
      if (wasBomb != HasBomb()) {
        RenderEntity();
      }
    }
  }
  public int countdown {
    get { return _countdown; }
    set {
      if (_countdown == value) return;

      var updateEntity = _countdown > 0 || value > 0;
      _countdown = value;
      switch (_countdown) {
        case 0:
          if (bombDigit > 0) {
            this.PostNotification(Notifications.BOMB_EXPLODED, currentDigit);
          }
          if (digitStatus == DigitStatus.Empty && bombDigit == solutionDigit) {
            currentDigit = bombDigit;
            Evaluate();
          }
          break;
        case 1:
          this.PostNotification(Notifications.BOMB_PRIMED);
          break;
      }

      if (updateEntity) {
        RenderEntity();
        RenderDigit();
      }
    }
  }
  public Point pos { get { return data.pos; } set { data.pos = value; } }
  public Vector2 center { get { return new Vector3(pos.x, pos.y); } }
  public int solutionDigit {
    get { return data.solution; }
    set { data.solution = value; RenderDigit(); }
  }
  public int currentDigit {
    get { return _currentDigit; }
    set { _currentDigit = value; if (_currentDigit < 0) _currentDigit = 0; Evaluate(); }
  }
  public int bombDigit {
    get { return _bombDigit; }
    set { _bombDigit = value; }
  }
  public DigitDisplayMode digitDisplayMode { get { return digit.displayMode; } set { digit.displayMode = value; } }
  [SerializeField] private int _currentDigit;
  [SerializeField] private int _bombDigit;
  [SerializeField] private DigitStatus _digitStatus;
  [SerializeField] private BombStatus _bombStatus;
  [SerializeField] private int _countdown;

  //commands
  public void Evaluate() {
    if (digitStatus == DigitStatus.Wall && currentDigit > 0) return;

    if (currentDigit <= 0) {
      digitStatus = DigitStatus.Empty;
    } else if (currentDigit > 0 && solutionDigit > 0) {
      digitStatus = currentDigit == solutionDigit ? DigitStatus.Confirmed : DigitStatus.Wall;
    } else {
      digitStatus = DigitStatus.Confirmed;
      Debug.Log("Confirmed a digit because no solution was defined.");
    }
  }
  public void DamageWall(int value) {
    if (digitStatus != DigitStatus.Wall) return;
    if (value <= 0) return;

    var newVal = currentDigit - value;
    currentDigit = Mathf.Max(0, newVal);
    if (currentDigit <= 0) digitStatus = DigitStatus.Empty;
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
      Evaluate();
    }
  }
  public void RenderTile() {
    tileRenderer.Render();
  }
  public void RenderWall() {
    if (wallRenderer) wallRenderer.Render();
  }
  public void RenderEntity() {
    if (tileEntity) tileEntity.Render();
  }
  public void RenderDigit() {
    digit.UpdateDigit();
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
    return digitStatus != DigitStatus.Wall && !HasBomb();
  }
  public bool IsWall() {
    return digitStatus == DigitStatus.Wall;
  }
  public bool BlocksVisibility() {
    return digitStatus == DigitStatus.Wall;
  }
  public bool HasBomb() {
    return bombStatus != BombStatus.None;
  }
  public bool IsEmpty() {
    return !HasBomb() && digitStatus == DigitStatus.Empty;
  }
}
