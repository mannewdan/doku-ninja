using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateModal : MenuState {
  protected GameObject modalPrefab { get { return owner.modalPrefab; } }

  public override void Enter() {
    base.Enter();
    GameObject newModal = Instantiate(modalPrefab);
    newModal.transform.SetParent(transform.parent);
    newModal.transform.localPosition = Vector3.zero;
    newModal.transform.localScale = Vector3.one;

    MenuController modal = newModal.GetComponent<MenuController>();
    modal.invoker = owner;
    owner.paused = true;
  }

  public override void OnUnpause() {
    owner.ChangeState<MenuStateRunning>();
  }
}
