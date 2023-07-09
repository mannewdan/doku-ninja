using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
  [SerializeField] GameObject ninjaPrefab;
  [SerializeField] GameObject skellyBasicPrefab;

  public Unit player;
  public List<Unit> enemies = new List<Unit>();

  public void Load(MapData data) {
    Clear();
    GameObject newNinja = Instantiate(ninjaPrefab);
    newNinja.transform.SetParent(transform);

    player = newNinja.GetComponent<Unit>();
    player.pos = data.spawn;
    player.Snap();

    for (int i = 0; i < data.enemies.Count; i++) {
      EnemyData ed = data.enemies[i];
      GameObject newEnemy = Instantiate(skellyBasicPrefab);
      newEnemy.transform.SetParent(transform);

      Unit enemy = newEnemy.GetComponent<Unit>();
      enemy.pos = ed.pos;
      enemy.Snap();
    }
  }
  public void Clear() {
    if (player) {
      Destroy(player.gameObject);
      player = null;
    }

    for (int i = enemies.Count - 1; i >= 0; i--) {
      if (enemies[i]) {
        Destroy(enemies[i].gameObject);
      }
    }
    enemies.Clear();
  }
}
