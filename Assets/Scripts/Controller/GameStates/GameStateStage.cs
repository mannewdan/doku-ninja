using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateStage : GameState {
  StageController controller;

  public override void Enter() {
    base.Enter();
    GameObject stageController = Instantiate(owner.stageControllerPrefab);
    stageController.transform.SetParent(owner.transform);
    stageController.transform.localPosition = Vector3.zero;
    stageController.transform.localEulerAngles = Vector3.zero;
    stageController.transform.localScale = Vector3.one;
    controller = stageController.GetComponent<StageController>();
  }

  protected override void OnDebug(object sender, object e) {
    Destroy(controller.gameObject);
    owner.ChangeState<GameStateStart>();
  }
}
