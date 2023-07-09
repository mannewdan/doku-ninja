using Type = System.Type;
using Array = System.Array;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditStateDraw : EditState {
  protected static readonly Type[] cycleOrder = new Type[] { typeof(EditStateGiven), typeof(EditStateSolution), typeof(EditStateUnit) };

  protected override void OnMoveRepeat(object sender, object e) {
    pos += ((InfoEventArgs<Point>)e).info;
    pos = pos.Clamp(0, mapData.width - 1, 0, mapData.height - 1);
    SnapMarker();
  }
  protected override void OnTab(object sender, object e) {
    var increment = ((InfoEventArgs<int>)e).info;
    var newIndex = Array.IndexOf(cycleOrder, this.GetType());
    if (newIndex < 0) {
      Debug.Log("Couldn't find type: " + this.GetType().ToString() + " inside of the cycleOrder list.");
      newIndex = 0;
    } else {
      newIndex += increment;
      if (newIndex > cycleOrder.Length - 1) newIndex = 0;
      if (newIndex < 0) newIndex = cycleOrder.Length - 1;
    }

    var baseMethod = typeof(EditController).GetMethod("ChangeState");
    var stateMethod = baseMethod.MakeGenericMethod(cycleOrder[newIndex]);
    stateMethod.Invoke(owner, null);
  }
}
