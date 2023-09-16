using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEdit : GameState {
  EditController controller;

  public override void Enter() {
    base.Enter();
    GameObject editController = Instantiate(owner.editControllerPrefab);
    editController.transform.SetParent(owner.transform);
    editController.transform.localPosition = Vector3.zero;
    editController.transform.localEulerAngles = Vector3.zero;
    editController.transform.localScale = Vector3.one;
    controller = editController.GetComponent<EditController>();
  }

  protected override void OnDebug(object sender, object e) {
    Destroy(controller.gameObject);
    owner.ChangeState<GameStateStart>();
  }
}
