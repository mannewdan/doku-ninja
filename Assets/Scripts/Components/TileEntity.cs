using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntity : MonoBehaviour {
  private const float SPARK_ROTATE_SPEED = 60.0f;

  [SerializeField] private GameObject spark;

  private Tile owner;
  private bool showSpark;
  private readonly List<Point> targetedTiles = new List<Point>();
  private Point pos { get { return owner.pos; } }
  private Grid grid { get { return owner.grid; } }
  private UnitManager units { get { return grid.units; } }
  private int countdown { get { return owner.countdown; } }
  private int currentDigit { get { return owner.currentDigit; } }
  private TileStatus status { get { return owner.status; } }

  void OnEnable() {
    this.AddObserver(TargetTiles, Notifications.MAP_WALL_CHANGED);
    this.AddObserver(TargetTiles, Notifications.BOMB_PRIMED, owner);
    this.AddObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
    this.AddObserver(DamageTargets, Notifications.BOMB_EXPLODED, owner);
  }
  void OnDisable() {
    this.RemoveObserver(TargetTiles, Notifications.MAP_WALL_CHANGED);
    this.RemoveObserver(TargetTiles, Notifications.BOMB_PRIMED, owner);
    this.RemoveObserver(ClearTargets, Notifications.BOMB_REMOVED, owner);
    this.RemoveObserver(DamageTargets, Notifications.BOMB_EXPLODED, owner);
  }

  protected void Awake() {
    owner = GetComponent<Tile>();
    if (!owner) owner = GetComponentInParent<Tile>();
  }
  protected void Start() {
    Render();
  }
  protected void Update() {
    if (showSpark) {
      spark.transform.localEulerAngles = Vector3.forward * Time.time * SPARK_ROTATE_SPEED;
      transform.localScale = Vector3.one * (1.0f + 0.035f * Mathf.Sin(Time.time * 8.0f));
    }
  }
  public void Render() {
    var coordinates = new Vector2(3, 3);

    switch (status) {
      case TileStatus.BoxBomb:
        coordinates = new Vector2(0, countdown == 2 ? 1 : 2);
        break;
      case TileStatus.StarBomb:
        coordinates = new Vector2(1, countdown == 2 ? 1 : 2);
        break;
    }

    MeshFilter mFilter = GetComponent<MeshFilter>();
    List<Vector2> uvs = new List<Vector2>(mFilter.mesh.uv);
    float t = 0.25f;
    uvs[0] = new Vector2(coordinates.x * t, coordinates.y * t);
    uvs[1] = new Vector2(coordinates.x * t + t, coordinates.y * t);
    uvs[2] = new Vector2(coordinates.x * t, coordinates.y * t + t);
    uvs[3] = new Vector2(coordinates.x * t + t, coordinates.y * t + t);

    Mesh mesh = mFilter.mesh;
    mesh.uv = uvs.ToArray();

    showSpark = countdown == 1;
    spark.gameObject.SetActive(showSpark);
    transform.localScale = Vector3.one;
  }
  public void DamageTargets(object sender = null, object data = null) {
    foreach (Point p in new List<Point>(targetedTiles)) {
      if (p == pos) continue;

      var tile = grid.tiles.ContainsKey(p) ? grid.tiles[p] : null;
      var unit = units.unitMap.ContainsKey(p) ? units.unitMap[p] : null;
      if (!tile) continue;
      if (unit) {
        unit.Harm(currentDigit);
      } else if (tile.status == TileStatus.Wall) {
        tile.DamageTile(currentDigit, true, true);
      } else if (tile.IsBomb()) {
        while (tile.countdown > 0) {
          tile.countdown--;
        }
      }
    }

    ClearTargets();
  }
  public void TargetTiles(object sender = null, object data = null) {
    ClearTargets();

    if (!owner.IsBomb()) return;

    targetedTiles.Add(pos);
    List<Point> candidates = new List<Point>();
    switch (status) {
      case TileStatus.BoxBomb:
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
      case TileStatus.StarBomb:
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
