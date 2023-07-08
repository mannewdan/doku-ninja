using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EditStateInit : EditState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Init());
  }

  IEnumerator<float> _Init() {
    grid.Load(new GridData());
    SnapMarker();
    yield return 0;
    owner.ChangeState<EditStateGiven>();
  }
}
