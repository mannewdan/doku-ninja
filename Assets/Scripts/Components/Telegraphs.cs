using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphInfo {
  public GameObject sender;
  public List<Point> targetedTiles;
  public TelegraphInfo(GameObject sender, List<Point> targetedTiles) {
    this.sender = sender;
    this.targetedTiles = targetedTiles;
  }
}
public class Telegraphs : MonoBehaviour {
  [SerializeField] GameObject telegraphPrefab;
  [SerializeField] Material telegraphMat;
  [SerializeField] Material highlightMat;
  [SerializeField] private UnitManager units;
  private readonly Dictionary<Point, Data> enemyTelegraphs = new Dictionary<Point, Data>();
  private readonly Dictionary<Point, Data> playerHighlights = new Dictionary<Point, Data>();
  private UnitController player { get { return units.player; } }

  class Data {
    public GameObject telegraphObject = null;
    public List<GameObject> senders = new List<GameObject>();
  }
  void OnEnable() {
    this.AddObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.AddObserver(AddTelegraph, Notifications.CARD_ACTIVE);
    this.AddObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
    this.AddObserver(RemoveTelegraph, Notifications.CARD_INACTIVE);
    this.AddObserver(HideSafeTelegraphs, Notifications.ENEMY_PHASE_START);
  }
  void OnDisable() {
    this.RemoveObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.RemoveObserver(AddTelegraph, Notifications.CARD_ACTIVE);
    this.RemoveObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
    this.RemoveObserver(RemoveTelegraph, Notifications.CARD_INACTIVE);
    this.RemoveObserver(HideSafeTelegraphs, Notifications.ENEMY_PHASE_START);
  }

  void AddTelegraph(object sender, object e) {
    if (e is TelegraphInfo tInfo) {
      var isPlayer = tInfo.sender == player.gameObject;
      var dictionary = isPlayer ? playerHighlights : enemyTelegraphs;

      foreach (Point pos in tInfo.targetedTiles) {
        if (!dictionary.ContainsKey(pos)) {
          dictionary.Add(pos, new Data());
          GameObject newTelegraph = Instantiate(telegraphPrefab);
          newTelegraph.transform.SetParent(transform);
          newTelegraph.transform.localPosition = new Vector3(pos.x, pos.y, isPlayer ? -1 : 0);
          dictionary[pos].telegraphObject = newTelegraph;

          Material mat = isPlayer ? highlightMat : telegraphMat;
          foreach (MeshRenderer mRenderer in newTelegraph.GetComponentsInChildren<MeshRenderer>()) {
            mRenderer.sharedMaterial = mat;
          }
        } else {
          dictionary[pos].telegraphObject.SetActive(true);
        }

        if (!dictionary[pos].senders.Contains(tInfo.sender)) dictionary[pos].senders.Add(tInfo.sender);
      }
    }
  }
  void RemoveTelegraph(object sender, object e) {
    if (e is TelegraphInfo tInfo) {
      var dictionary = tInfo.sender == player.gameObject ? playerHighlights : enemyTelegraphs;

      foreach (Point pos in tInfo.targetedTiles) {
        if (!dictionary.ContainsKey(pos)) continue;
        if (dictionary[pos].senders.Contains(tInfo.sender)) dictionary[pos].senders.Remove(tInfo.sender);

        for (int i = dictionary[pos].senders.Count - 1; i >= 0; i--) {
          UnitController unit = dictionary[pos].senders[i]?.GetComponent<UnitController>();
          if (unit == null || !unit.isAlive) dictionary[pos].senders.Remove(unit.gameObject);
        }

        if (dictionary[pos].senders.Count == 0) dictionary[pos].telegraphObject.SetActive(false);
      }
    }
  }
  void HideSafeTelegraphs(object sender, object e) {
    foreach (KeyValuePair<Point, Data> t in enemyTelegraphs) {
      if (t.Key != player.pos) t.Value.telegraphObject.SetActive(false);
    }
  }
}
