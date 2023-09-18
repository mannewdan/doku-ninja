using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EditStateInit : EditState {
  public override void Enter() {
    base.Enter();
    Timing.RunCoroutine(_Init().CancelWith(owner.gameObject));
  }

  IEnumerator<float> _Init() {
    grid.Load(new MapData());
    pos = new Point(0, 0);
    yield return 0;
    owner.ChangeState<EditStateGiven>();
  }
}
