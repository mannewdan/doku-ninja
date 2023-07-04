using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour {
  [SerializeField] private TileRenderer tileRenderer;
  [SerializeField] private Digit digit;

  public Grid grid;
  public TileData data;
  public Point pos { get { return data.pos; } set { data.pos = value; } }
  public Vector2 center { get { return new Vector3(pos.x, pos.y); } }
  public int solutionDigit {
    get { return data.solution; }
    set { data.solution = value; digit.UpdateDigit(); }
  }
  public int currentDigit {
    get { return _currentDigit; }
    set { _currentDigit = value; digit.UpdateDigit(); }
  }
  private int _currentDigit;

  public void Load(TileData data, Grid grid) {
    this.data = data;
    this.grid = grid;
    currentDigit = data.given;
    Snap();

    if (data.type == TileType.Cliff) {
      Destroy(digit.gameObject);
    }
  }
  public void RenderTile() {
    tileRenderer.SetMaterialAndUVs();
  }
  public void SetDisplayMode(DigitDisplayMode displayMode) {
    digit.displayMode = displayMode;
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
