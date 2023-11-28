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
    } else {
      dataToLoad = new MapData();
    }

    mainRoutine = Timing.RunCoroutine(_Load().CancelWith(gameObject));
  }

  IEnumerator<float> _Load() {
    owner.mapData = dataToLoad;
    yield return 0;
    owner.ChangeState<EditStateGiven>();
  }
}
