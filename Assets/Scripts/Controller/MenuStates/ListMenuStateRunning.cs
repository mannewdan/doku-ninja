using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuStateRunning : MenuStateRunning {
  new protected ListMenuController owner { get { return (ListMenuController)base.owner; } }

  protected override void OnMove(object sender, object e) {
    if (e is Point delta) {
      //vertical
      if (delta.y != 0) {
        owner.selection -= delta.y;
      }

      //horizontal
      if (delta.x != 0) {
        //pass input to the current element
      }
    }
  }

  protected override void OnConfirm(object sender, object e) {
    //execute function on current element
  }
}
