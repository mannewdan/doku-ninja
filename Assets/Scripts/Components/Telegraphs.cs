using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphInfo {
  public GameObject sender;
  public List<Point> targetedTiles;
  public bool isBomb;
  public TelegraphInfo(GameObject sender, List<Point> targetedTiles, bool isBomb = false) {
    this.sender = sender;
    this.targetedTiles = targetedTiles;
    this.isBomb = isBomb;
  }
}
public class Telegraphs : MonoBehaviour {
  [SerializeField] GameObject telegraphPrefab;
  [SerializeField] Material enemyTelegraphMat;
  [SerializeField] Material bombTelegraphMat;
  [SerializeField] Material highlightMat;
  [SerializeField] private UnitManager units;
  private readonly Dictionary<Point, Data> enemyTelegraphs = new Dictionary<Point, Data>();
  private readonly Dictionary<Point, Data> bombTelegraphs = new Dictionary<Point, Data>();
  private readonly Dictionary<Point, Data> playerHighlights = new Dictionary<Point, Data>();
  private UnitController player { get { return units.player; } }

  class Data {
    public GameObject telegraphObject = null;
    public List<GameObject> senders = new List<GameObject>();
  }
  void OnEnable() {
    this.AddObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.AddObserver(AddTelegraph, Notifications.BOMB_ADD_TARGET);
    this.AddObserver(AddTelegraph, Notifications.CARD_ACTIVE);
    this.AddObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
    this.AddObserver(RemoveTelegraph, Notifications.BOMB_REMOVE_TARGET);
    this.AddObserver(RemoveTelegraph, Notifications.CARD_INACTIVE);
    this.AddObserver(HideSafeTelegraphs, Notifications.ENEMY_PHASE_START);
    this.AddObserver(ResetAll, Notifications.RESET);
  }
  void OnDisable() {
    this.RemoveObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.RemoveObserver(AddTelegraph, Notifications.BOMB_ADD_TARGET);
    this.RemoveObserver(AddTelegraph, Notifications.CARD_ACTIVE);
    this.RemoveObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
    this.RemoveObserver(RemoveTelegraph, Notifications.BOMB_REMOVE_TARGET);
    this.RemoveObserver(RemoveTelegraph, Notifications.CARD_INACTIVE);
    this.RemoveObserver(HideSafeTelegraphs, Notifications.ENEMY_PHASE_START);
    this.RemoveObserver(ResetAll, Notifications.RESET);
  }

  void AddTelegraph(object sender, object e) {
    if (e is TelegraphInfo tInfo) {
      var isPlayer = tInfo.sender == player.gameObject;

      var dictionary = playerHighlights;
      var mat = highlightMat;
      var scale = 1.0f;
      if (tInfo.isBomb) {
        dictionary = bombTelegraphs;
        mat = bombTelegraphMat;
        scale = 0.85f;
      } else if (!isPlayer) {
        dictionary = enemyTelegraphs;
        mat = enemyTelegraphMat;
      }

      foreach (Point pos in tInfo.targetedTiles) {
        if (!dictionary.ContainsKey(pos)) {
          dictionary.Add(pos, new Data());
          GameObject newTelegraph = Instantiate(telegraphPrefab);
          newTelegraph.transform.SetParent(transform);
          newTelegraph.transform.localPosition = new Vector3(pos.x, pos.y, isPlayer ? -1 : 0);
          newTelegraph.transform.localScale = Vector3.one * scale;
          dictionary[pos].telegraphObject = newTelegraph;

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
      var dictionary = tInfo.sender == player.gameObject ? playerHighlights : tInfo.isBomb ? bombTelegraphs : enemyTelegraphs;

      foreach (Point pos in tInfo.targetedTiles) {
        if (!dictionary.ContainsKey(pos)) continue;
        if (dictionary[pos].senders.Contains(tInfo.sender)) dictionary[pos].senders.Remove(tInfo.sender);

        for (int i = dictionary[pos].senders.Count - 1; i >= 0; i--) {
          GameObject s = dictionary[pos].senders[i];
          UnitController unit = s ? s.GetComponent<UnitController>() : null;
          if (unit == null || !unit.isAlive) {
            dictionary[pos].senders.RemoveAt(i);
            continue;
          }
        }

        if (dictionary[pos].senders.Count == 0) dictionary[pos].telegraphObject.SetActive(false);
      }
    }
  }
  void ResetAll(object sender, object e) {
    ResetTelegraphs(playerHighlights);
    ResetTelegraphs(enemyTelegraphs);
    ResetTelegraphs(bombTelegraphs);
  }
  void ResetTelegraphs(Dictionary<Point, Data> telegraphs) {
    foreach (Point pos in telegraphs.Keys) {
      telegraphs[pos].senders.Clear();
      telegraphs[pos].telegraphObject.SetActive(false);
    }
  }
  void HideSafeTelegraphs(object sender, object e) {
    foreach (KeyValuePair<Point, Data> t in enemyTelegraphs) {
      if (t.Key != player.pos) t.Value.telegraphObject.SetActive(false);
    }
  }
}
