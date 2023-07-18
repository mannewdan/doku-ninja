using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
  [SerializeField] GameObject ninjaPrefab;
  [SerializeField] GameObject skellyBasicPrefab;

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

  private UnitController _player;

  void Start() {
    player.pos = spawn;
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
    enemies.Add(enemy);
    return enemy;
  }
}
