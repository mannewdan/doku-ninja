using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {
  public virtual State currentState {
    get { return _currentState; }
    set { Transition(value); }
  }
  protected State _currentState;
  public bool paused {
    get { return _paused; }
    set {
      if (_paused == value) return;
      _paused = value;
      _currentState.paused = _paused;
    }
  }
  [SerializeField] protected bool _paused;
  protected bool _inTransition;
  protected object _stateData;
  public object stateData { get { return _stateData; } set { _stateData = value; } }

  [SerializeField] private string debugCurrentState;

  public virtual T GetState<T>() where T : State {
    T target = GetComponent<T>();
    return target ??= gameObject.AddComponent<T>();
  }
  public virtual void ChangeState<T>() where T : State {
    currentState = GetState<T>();
  }
  protected virtual void Transition(State value) {
    if (_currentState == value || _inTransition) return;

    _inTransition = true;
    if (_currentState != null) _currentState.Exit();
    _currentState = value;
    debugCurrentState = value.ToString();
    if (_currentState != null) {
      _currentState.Enter();
      _currentState.paused = paused;
    }
    _inTransition = false;
    _stateData = null;
  }
}
