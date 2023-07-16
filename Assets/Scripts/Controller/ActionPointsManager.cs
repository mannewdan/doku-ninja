using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointsManager : MonoBehaviour {
  public const int BASE_AP_PER_TURN = 2;
  public const int BASE_AP_MAX = 3;

  public int ap { get { return _ap; } }
  [SerializeField] private int _ap;

  void OnEnable() {
    this.AddObserver(IncrementAP, Notifications.PLAYER_PHASE_START);
  }
  void OnDisable() {
    this.RemoveObserver(IncrementAP, Notifications.PLAYER_PHASE_START);
  }

  void IncrementAP(object sender, object e) {
    _ap += BASE_AP_PER_TURN;
    if (_ap > BASE_AP_MAX) _ap = BASE_AP_MAX;
  }
  public bool SpendAP(int amount = 1) {
    if (_ap < amount) {
      Debug.Log("Not enough AP!");
      return false;
    }
    _ap -= amount;

    this.PostNotification(Notifications.PLAYER_SPENT_AP);
    return true;
  }
}
