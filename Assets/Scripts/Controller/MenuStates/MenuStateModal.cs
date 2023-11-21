using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class MenuStateModal : MenuState {
  protected GameObject modalPrefab { get { return owner.nextModal; } }

  public override void Enter() {
    base.Enter();

    if (modalPrefab == null) {
      Debug.Log("Couldn't open a modal because it wasn't defined");
      mainRoutine = Timing.RunCoroutine(_Revert().CancelWith(gameObject));
      return;
    }

    GameObject newModal = Instantiate(modalPrefab);
    newModal.transform.SetParent(transform.parent);
    newModal.transform.localPosition = Vector3.zero;
    newModal.transform.localScale = Vector3.one;

    owner.currentModal = newModal;

    MenuController modal = newModal.GetComponent<MenuController>();
    modal.invoker = owner;
    owner.paused = true;
  }

  public override void OnUnpause() {
    owner.RevertState();
  }
  protected IEnumerator<float> _Revert() {
    yield return 0;
    owner.paused = false;
    owner.RevertState();
  }
}
