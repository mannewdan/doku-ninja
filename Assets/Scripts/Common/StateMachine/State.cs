using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
  public virtual void Enter() {
    AddObservers();
  }
  public virtual void Exit() {
    RemoveObservers();
  }
  protected virtual void OnDestroy() {
    RemoveObservers();
  }
  protected virtual void AddObservers() {

  }
  protected virtual void RemoveObservers() {

  }
}
