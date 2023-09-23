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
      _status = value;
      switch (_status) {
        case TileStatus.Confirmed: digitDisplayMode = DigitDisplayMode.Confirmed; break;
        case TileStatus.Wall: digitDisplayMode = DigitDisplayMode.Wall; break;
        default: digitDisplayMode = DigitDisplayMode.Default; break;
      }
      RenderWall();
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
  private TileStatus _status;

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
    tileRenderer.SetMaterialAndUVs();
  }
  public void RenderWall() {
    if (wallRenderer) wallRenderer.SetMaterialAndUVs();
  }
  void Snap() {
    transform.localPosition = center;
    transform.localScale = new Vector3(1, 1, 1);
  }
  public TileData GatherData() {
    if (data.type == TileType.Cliff) return null;
    data.given = currentDigit;
    return data;
  }
}
