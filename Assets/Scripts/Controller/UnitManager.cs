using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
  public GameObject activeIndicatorPrefab;
  [SerializeField] private GameObject ninjaPrefab;
  [SerializeField] private GameObject skellyBasicPrefab;
  [SerializeField] private Grid grid;
  [SerializeField] private Telegraphs telegraphs;

  public Point spawn;
  public UnitController player {
    get {
      if (_player == null) {
        _player = NewUnit(spawn, ninjaPrefab);
        _player.isPlayer = true;
      }
      return _player;
    }
  }
  public List<UnitController> enemies = new List<UnitController>();
  public Dictionary<Point, UnitController> unitMap = new Dictionary<Point, UnitController>();
  private UnitController _player;

  void Start() {
    player.pos = spawn;
    player.renderer.Snap();
  }
  void OnEnable() {
    this.AddObserver(UpdateMap, Notifications.UNIT_MOVED);
    this.AddObserver(UpdateMap, Notifications.UNIT_DESTROYED);
  }
  void OnDisable() {
    this.RemoveObserver(UpdateMap, Notifications.UNIT_MOVED);
    this.RemoveObserver(UpdateMap, Notifications.UNIT_DESTROYED);
  }

  private void UpdateMap(object sender = null, object e = null) {
    unitMap.Clear();
    unitMap.Add(player.pos, player);
    for (int i = enemies.Count - 1; i >= 0; i--) {
      if (!enemies[i] || !enemies[i].isAlive) { enemies.RemoveAt(i); continue; }
      if (unitMap.ContainsKey(enemies[i].pos)) {
        Debug.LogError("Tile has two units occupying it!", enemies[i].gameObject);
        continue;
      }
      unitMap.Add(enemies[i].pos, enemies[i]);
    }
  }
  public void Load(MapData data) {
    Clear();

    spawn = data.spawn;
    player.pos = spawn;

    for (int i = 0; i < data.enemies.Count; i++) {
      EnemyData ed = data.enemies[i];
      UnitController enemy = NewUnit(ed.pos, skellyBasicPrefab);
      enemies.Add(enemy);
    }

    UpdateMap();
  }
  public void Clear() {
    spawn = new Point(0, 0);
    player.pos = spawn;

    for (int i = enemies.Count - 1; i >= 0; i--) {
      if (enemies[i]) {
        Destroy(enemies[i].gameObject);
      }
    }
    enemies.Clear();
  }
  public void GatherData(ref MapData mapData) {
    mapData.spawn = spawn;
    mapData.enemies = new List<EnemyData>();
    foreach (UnitController unit in enemies) {
      if (unit == null) continue;
      mapData.enemies.Add(new EnemyData() { pos = unit.pos });
    }
  }
  public void SetSpawn(Point pos) {
    RemoveEnemy(pos);
    spawn = pos;
    player.pos = spawn;
    player.renderer.Snap();
  }
  public void PlaceEnemy(Point pos) {
    if (spawn == pos) return;
    RemoveEnemy(pos);

    UnitController enemy = NewUnit(pos, skellyBasicPrefab);
    enemies.Add(enemy);
    UpdateMap();
  }
  public void RemoveEnemy(Point pos) {
    for (int i = enemies.Count - 1; i >= 0; i--) {
      if (enemies[i].pos == pos) {
        Destroy(enemies[i].gameObject);
        enemies.RemoveAt(i);
        break;
      }
    }
  }
  public bool IsOccupied(Point p) {
    if (player.pos == p) return true;
    foreach (UnitController u in enemies) {
      if (u.pos == p) return true;
    }
    return false;
  }
  public UnitController NewUnit(Point pos, GameObject prefab) {
    GameObject newUnit = Instantiate(prefab);
    newUnit.transform.SetParent(transform);

    UnitController unit = newUnit.GetComponent<UnitController>();
    unit.pos = pos;
    unit.grid = grid;
    unit.units = this;
    unit.renderer.Snap();

    return unit;
  }
}
