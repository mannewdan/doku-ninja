using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsManager : MonoBehaviour {
  public const int BASE_AP_PER_TURN = 2;
  public const int BASE_AP_MAX = 3;

  [SerializeField] private GameObject pointIndicatorPrefab;
  public readonly List<PointIndicator> indicators = new List<PointIndicator>();

  public int ap { get { return _ap; } }
  public int maxAP { get { return BASE_AP_MAX + maxAPBonus; } }
  public int maxAPBonus {
    get { return _maxAPBonus; }
    set {
      _maxAPBonus = value;
      if (indicators.Count < maxAP) {
        for (int i = 0; i < maxAP - indicators.Count; i++) {
          BuildIndicator();
        }
      }
    }
  }
  [SerializeField] private int _ap;
  [SerializeField] private int _maxAPBonus;

  void Start() {
    for (int i = 0; i < maxAP; i++) {
      BuildIndicator();
    }
  }
  void OnEnable() {
    this.AddObserver(IncrementAP, Notifications.PLAYER_PHASE_START);
  }
  void OnDisable() {
    this.RemoveObserver(IncrementAP, Notifications.PLAYER_PHASE_START);
  }

  //commands
  public bool SpendAP(int amount = 1) {
    if (_ap < amount) {
      Debug.Log("Not enough AP!");
      return false;
    }
    _ap -= amount;

    this.PostNotification(Notifications.PLAYER_SPENT_AP, ap);
    return true;
  }
  void IncrementAP(object sender, object e) {
    _ap += BASE_AP_PER_TURN;
    if (_ap > maxAP) _ap = maxAP;
    this.PostNotification(Notifications.PLAYER_GAINED_AP, ap);
  }

  //indicators
  void BuildIndicator() {
    GameObject newIndicator = Instantiate(pointIndicatorPrefab);
    newIndicator.transform.SetParent(transform);

    PointIndicator indicator = newIndicator.GetComponent<PointIndicator>();
    indicators.Add(indicator);
    indicator.Initialize(this);
    this.PostNotification(Notifications.PLAYER_CHANGED_AP_MAX);
  }
}
