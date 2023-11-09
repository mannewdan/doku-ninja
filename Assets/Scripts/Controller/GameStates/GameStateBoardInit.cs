using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateStageInit : GameState {
  public override void Enter() {
    base.Enter();
    mainRoutine = Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  IEnumerator<float> _Start() {
    GameObject stageController = Instantiate(owner.stageControllerPrefab);
    stageController.transform.SetParent(owner.transform);
    stageController.transform.localPosition = Vector3.zero;
    stageController.transform.localEulerAngles = Vector3.zero;
    stageController.transform.localScale = Vector3.one;
    owner.currentBoard = stageController.GetComponent<StageController>();

    yield return 0;
    owner.ChangeState<GameStateStageRunning>();
  }
}
