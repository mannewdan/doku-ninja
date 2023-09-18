using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EditStateLoad : EditState {
  MapData dataToLoad;

  public override void Enter() {
    base.Enter();
    if (owner.stateData is MapData mapData) {
      dataToLoad = mapData;
      Timing.RunCoroutine(_Load().CancelWith(gameObject));
    } else {
      Debug.LogError(owner.stateData == null ? "Load data was not provided" : "Failed to load data");
      Timing.RunCoroutine(_Failure().CancelWith(gameObject));
    }
  }

  IEnumerator<float> _Load() {
    owner.mapData = dataToLoad;
    yield return 0;
    owner.ChangeState<EditStateGiven>();
  }
  IEnumerator<float> _Failure() {
    yield return 0;
    owner.ChangeState<EditStateInit>();
  }
}
