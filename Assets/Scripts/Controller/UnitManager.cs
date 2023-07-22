using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
  [SerializeField] private GameObject ninjaPrefab;
  [SerializeField] private GameObject skellyBasicPrefab;
  [SerializeField] private Grid grid;

  public Point spawn;
  public UnitController player {
    get {
      if (_player == null) {
        GameObject newNinja = Instantiate(ninjaPrefab);
        newNinja.transform.SetParent(transform);
        _player = newNinja.GetComponent<UnitController>();
        _player.pos = spawn;
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
  void OnEnable() { AddObservers(); }
  void OnDisable() { RemoveObservers(); }
  void OnDestroy() { RemoveObservers(); }
  void AddObservers() {
    this.AddObserver(UpdateMap, Notifications.UNIT_MOVED);
    this.AddObserver(UpdateMap, Notifications.UNIT_DESTROYED);
  }
  void RemoveObservers() {
    this.RemoveObserver(UpdateMap, Notifications.UNIT_MOVED);
    this.RemoveObserver(UpdateMap, Notifications.UNIT_DESTROYED);
  }

  private void UpdateMap(object sender, object e) {
    unitMap.Clear();
    unitMap.Add(player.pos, player);
    for (int i = enemies.Count - 1; i >= 0; i--) {
      if (!enemies[i]) { enemies.RemoveAt(i); continue; }
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
      NewEnemy(ed.pos);
    }
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
    NewEnemy(pos);
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
  public UnitController NewEnemy(Point pos) {
    GameObject newEnemy = Instantiate(skellyBasicPrefab);
    newEnemy.transform.SetParent(transform);

    UnitController enemy = newEnemy.GetComponent<UnitController>();
    enemy.pos = pos;
    enemy.renderer.Snap();
    enemy.grid = grid;
    enemy.units = this;
    enemies.Add(enemy);
    return enemy;
  }
  public bool IsOccupied(Point p) {
    if (player.pos == p) return true;
    foreach (UnitController u in enemies) {
      if (u.pos == p) return true;
    }
    return false;
  }
}
