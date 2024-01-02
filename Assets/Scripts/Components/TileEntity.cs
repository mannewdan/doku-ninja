using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntity : MonoBehaviour {
  private Tile owner; public Tile tile { get { return owner; } }
  private readonly List<Point> targetedTiles = new List<Point>();
  public Point pos { get { return owner.pos; } }
  public Grid grid { get { return owner.grid; } }
  public UnitManager units { get { return grid.units; } }
  public int bombValue { get { return owner.bombDigit; } }
  public DigitStatus digitStatus { get { return owner.digitStatus; } }
  public BombStatus bombStatus { get { return owner.bombStatus; } }

  public int countdown {
    get { return _countdown; }
    set {
      if (_countdown == value) return;

      _countdown = value;
      this.PostNotification(Notifications.BOMB_COUNTDOWN, _countdown);
      switch (_countdown) {
        case 0:
          if (bombValue > 0) {
            this.PostNotification(Notifications.BOMB_EXPLODED, bombValue);
            DamageTargets();

            if (digitStatus == DigitStatus.Empty && bombValue == owner.solutionDigit) {
              owner.currentDigit = bombValue;
              owner.grid.ValidateBoard();
            }

            tile.bombStatus = BombStatus.None;
            tile.bombDigit = 0;
          }
          break;
        case 1:
          this.PostNotification(Notifications.BOMB_PRIMED);
          TargetTiles();
          break;
      }
    }
  }
  [SerializeField] private int _countdown;

  void OnEnable() {
    this.AddObserver(TargetTiles, Notifications.MAP_WALL_CHANGED);
    this.AddObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
  }
  void OnDisable() {
    this.RemoveObserver(TargetTiles, Notifications.MAP_WALL_CHANGED);
    this.RemoveObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
  }

  protected void Awake() {
    owner = GetComponent<Tile>();
    if (!owner) owner = GetComponentInParent<Tile>();
  }

  public void DamageTargets(object sender = null, object data = null) {
    foreach (Point p in new List<Point>(targetedTiles)) {
      if (p == pos) continue;
      var tile = grid.tiles.ContainsKey(p) ? grid.tiles[p] : null;
      var unit = units.unitMap.ContainsKey(p) ? units.unitMap[p] : null;
      if (!tile) continue;
      if (unit) {
        unit.Harm(bombValue);
      } else if (tile.digitStatus == DigitStatus.Wall) {
        tile.DamageWall(bombValue);
      } else if (tile.HasBomb()) {
        while (tile.countdown > 0) {
          tile.countdown--;
        }
      }
    }

    ClearTargets();
  }
  public void TargetTiles(object sender = null, object data = null) {
    ClearTargets();

    if (!owner.HasBomb()) return;

    targetedTiles.Add(pos);
    List<Point> candidates = new List<Point>();
    switch (bombStatus) {
      case BombStatus.Box:
        //n, e, s, w, nw, ne, sw, se
        candidates.AddRange(new List<Point>() {
          new Point(pos.x, pos.y + 1),
          new Point(pos.x + 1, pos.y),
          new Point(pos.x, pos.y - 1),
          new Point(pos.x - 1, pos.y),
          new Point(pos.x - 1, pos.y + 1),
          new Point(pos.x + 1, pos.y + 1),
          new Point(pos.x - 1, pos.y - 1),
          new Point(pos.x + 1, pos.y - 1)
        });
        break;
      case BombStatus.Star:
        for (int x = pos.x - 1; x >= 0; x--) {
          if (TryAddTile(new Point(x, pos.y), candidates)) break;
        }
        for (int x = pos.x + 1; x < grid.width; x++) {
          if (TryAddTile(new Point(x, pos.y), candidates)) break;
        }
        for (int y = pos.y - 1; y >= 0; y--) {
          if (TryAddTile(new Point(pos.x, y), candidates)) break;
        }
        for (int y = pos.y + 1; y < grid.height; y++) {
          if (TryAddTile(new Point(pos.x, y), candidates)) break;
        }
        break;
    }

    foreach (Point p in candidates) {
      if (grid.InBounds(p)) targetedTiles.Add(p);
    }

    if (targetedTiles.Count > 0) {
      gameObject.PostNotification(Notifications.BOMB_ADD_TARGET, new TelegraphInfo(gameObject, targetedTiles, true));
    }
  }
  public void ClearTargets(object sender = null, object data = null) {
    if (targetedTiles.Count > 0) {
      gameObject.PostNotification(Notifications.BOMB_REMOVE_TARGET, new TelegraphInfo(gameObject, targetedTiles, true));
      targetedTiles.Clear();
    }
  }
  bool TryAddTile(Point p, List<Point> candidates) {
    if (grid.InBounds(p)) {
      candidates.Add(p);
      return grid.BlocksVisibility(p);
    } else return false;
  }
}
