using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuStateRunning : MenuStateRunning {
  new protected ListMenuController owner { get { return (ListMenuController)base.owner; } }

  protected override void OnMove(object sender, object e) {
    if (e is Point delta) {
      if (owner.orientation == ListMenuController.Orientation.Vertical && delta.y != 0) {
        owner.selection -= delta.y;
      }
      if (owner.orientation == ListMenuController.Orientation.Horizontal && delta.x != 0) {
        owner.selection += delta.x;
      }

      if (owner.orientation == ListMenuController.Orientation.Vertical && delta.x != 0) {
        //pass input to element
      }
      if (owner.orientation == ListMenuController.Orientation.Horizontal && delta.y != 0) {
        //pass input to element
      }
    }
  }

  protected override void OnConfirm(object sender, object e) {
    var element = owner.currentElement;
    if (element) {
      element.Execute();
    }
  }
}
