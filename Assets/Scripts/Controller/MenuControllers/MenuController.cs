using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : StateMachine {
  [SerializeField] GameObject confirmationPrefab;

  public StateMachine invoker;
  public GameObject nextModal;
  public GameObject currentModal;

  protected virtual void Start() {
    ChangeState<MenuStateRunning>();
  }
  public virtual void Close() {
    invoker.paused = false;
    DestroyImmediate(gameObject);
  }
  public virtual void ConfirmationModal(string prompt, Action action) {
    nextModal = confirmationPrefab;
    ChangeState<MenuStateModal>();

    if (currentModal) {
      var modal = currentModal.GetComponent<ConfirmationModalController>();
      if (modal) {
        modal.Initialize(prompt, action);
      }
    }
  }
}
