using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public abstract class State : MonoBehaviour {
  public bool paused {
    get { return _paused; }
    set {
      if (_paused == value) return;
      _paused = value;
      if (_paused) {
        RemoveInputObservers();
        Timing.PauseCoroutines(mainRoutine);
      } else {
        AddInputObservers();
        Timing.ResumeCoroutines(mainRoutine);
        OnUnpause();
      }
    }
  }
  [SerializeField] protected bool _paused;
  protected CoroutineHandle mainRoutine;

  public virtual void Enter() {
    AddLogicObservers();
    AddInputObservers();
  }
  public virtual void Exit() {
    RemoveLogicObservers();
    RemoveInputObservers();
    _paused = false;
  }
  protected virtual void OnDestroy() {
    RemoveLogicObservers();
    RemoveInputObservers();
  }
  protected virtual void AddLogicObservers() { }
  protected virtual void RemoveLogicObservers() { }
  protected virtual void AddInputObservers() { }
  protected virtual void RemoveInputObservers() { }
  public virtual void OnUnpause() { }
  public virtual bool IsPausable() { return true; }
}
