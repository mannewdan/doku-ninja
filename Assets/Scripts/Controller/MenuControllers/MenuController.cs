using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : StateMachine {
  [SerializeField] GameObject confirmationPrefab;

  [NonSerialized] public StateMachine invoker;
  [NonSerialized] public GameObject nextModal;
  [NonSerialized] public GameObject currentModal;

  public bool inputCloseable = true;

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
