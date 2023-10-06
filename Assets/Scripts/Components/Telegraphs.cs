using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphInfo {
  public UnitController unit;
  public List<Point> targetedTiles;
  public TelegraphInfo(UnitController unit, List<Point> targetedTiles) {
    this.unit = unit;
    this.targetedTiles = targetedTiles;
  }
}
public class Telegraphs : MonoBehaviour {
  [SerializeField] GameObject telegraphPrefab;
  private readonly Dictionary<Point, Data> telegraphs = new Dictionary<Point, Data>();

  class Data {
    public GameObject telegraphObject = null;
    public List<UnitController> units = new List<UnitController>();
  }

  void Start() {
    this.AddObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.AddObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
  }
  void OnEnable() {
    this.AddObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.AddObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
  }
  void OnDestroy() {
    this.RemoveObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.RemoveObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
  }
  void OnDisable() {
    this.RemoveObserver(AddTelegraph, Notifications.UNIT_ADD_TARGET);
    this.RemoveObserver(RemoveTelegraph, Notifications.UNIT_REMOVE_TARGET);
  }

  void AddTelegraph(object sender, object e) {
    if (e is TelegraphInfo tInfo) {
      foreach (Point pos in tInfo.targetedTiles) {
        if (!telegraphs.ContainsKey(pos)) {
          telegraphs.Add(pos, new Data());
          GameObject newTelegraph = Instantiate(telegraphPrefab);
          newTelegraph.transform.SetParent(transform);
          newTelegraph.transform.localPosition = new Vector3(pos.x, pos.y);
          telegraphs[pos].telegraphObject = newTelegraph;
        } else {
          telegraphs[pos].telegraphObject.SetActive(true);
        }

        if (!telegraphs[pos].units.Contains(tInfo.unit)) telegraphs[pos].units.Add(tInfo.unit);
      }
    }
  }
  void RemoveTelegraph(object sender, object e) {
    if (e is TelegraphInfo tInfo) {
      foreach (Point pos in tInfo.targetedTiles) {
        if (!telegraphs.ContainsKey(pos)) continue;
        if (telegraphs[pos].units.Contains(tInfo.unit)) telegraphs[pos].units.Remove(tInfo.unit);

        for (int i = telegraphs[pos].units.Count - 1; i >= 0; i--) {
          UnitController unit = telegraphs[pos].units[i];
          if (unit == null || !unit.isAlive) telegraphs[pos].units.Remove(unit);
        }

        if (telegraphs[pos].units.Count == 0) telegraphs[pos].telegraphObject.SetActive(false);
      }
    }
  }
}
