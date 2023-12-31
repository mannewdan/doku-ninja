using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class StageStateLoad : StageState {
  MapData dataToLoad;

  public override void Enter() {
    base.Enter();
    if (owner.stateData is MapData mapData) {
      dataToLoad = mapData;
      mainRoutine = Timing.RunCoroutine(_Load().CancelWith(gameObject));
    } else {
      Debug.LogError(owner.stateData == null ? "Load data was not provided" : "Failed to load data");
      mainRoutine = Timing.RunCoroutine(_Failure().CancelWith(gameObject));
    }
  }

  IEnumerator<float> _Load() {
    yield return 0;
    grid.Load(dataToLoad);
    units.Load(dataToLoad);
    deck.BuildDeck(dataToLoad.width);
    yield return 0;
    owner.ChangeState<StageStatePlayerStart>();
  }
  IEnumerator<float> _Failure() {
    yield return 0;
    Debug.LogError("Stage controller is stuck because an exit plan on failed load has not yet been implemented.");
  }
}
