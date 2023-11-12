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
        mounting = false;
      } else {
        AddInputObserversDelayed();
        Timing.ResumeCoroutines(mainRoutine);
        OnUnpause();
      }
    }
  }
  protected bool mounting {
    get { return _mounting; }
    set {
      if (_mounting && !value) {
        Timing.KillCoroutines(observersRoutine);
      }
      _mounting = value;
    }
  }
  [SerializeField] protected bool _paused;
  protected bool _mounting;
  protected CoroutineHandle mainRoutine;
  protected CoroutineHandle observersRoutine;

  public virtual void Enter() {
    AddLogicObservers();
    AddInputObserversDelayed();
  }
  public virtual void Exit() {
    RemoveLogicObservers();
    RemoveInputObservers();
    _paused = false;
    mounting = false;
  }
  protected virtual void OnDestroy() {
    RemoveLogicObservers();
    RemoveInputObservers();
    mounting = false;
  }
  protected virtual void AddLogicObservers() { }
  protected virtual void RemoveLogicObservers() { }
  protected virtual void AddInputObservers() { }
  protected virtual void RemoveInputObservers() { }
  public virtual void OnUnpause() { }
  public virtual bool IsPausable() { return !mounting; }

  private void AddInputObserversDelayed() {
    mounting = false;
    observersRoutine = Timing.RunCoroutine(_AddInputObserversDelayed());
    mounting = true;
  }
  private IEnumerator<float> _AddInputObserversDelayed() {
    yield return 0;
    AddInputObservers();
    mounting = false;
  }
}
