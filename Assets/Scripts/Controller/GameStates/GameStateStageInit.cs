using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class GameStateStageInit : GameState {
  StageController controller;

  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Start().CancelWith(gameObject));
  }

  protected override void OnDebug(object sender, object e) {
    Destroy(controller.gameObject);
    owner.ChangeState<GameStateStart>();
  }

  IEnumerator<float> _Start() {
    GameObject stageController = Instantiate(owner.stageControllerPrefab);
    stageController.transform.SetParent(owner.transform);
    stageController.transform.localPosition = Vector3.zero;
    stageController.transform.localEulerAngles = Vector3.zero;
    stageController.transform.localScale = Vector3.one;
    controller = stageController.GetComponent<StageController>();

    yield return 0;
    owner.ChangeState<GameStateStageRunning>();
  }
}
