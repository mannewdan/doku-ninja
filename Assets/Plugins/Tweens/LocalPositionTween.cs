using Tweens.Core;
using UnityEngine;

namespace Tweens {
  public static class LocalPositionTween {
    public static Tween<Vector3> TweenLocalPosition(this Component self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver>(self).Finalize(to, duration);

    public static Tween<Vector3> TweenLocalPosition(this GameObject self, Vector3 to, float duration) =>
      Tween<Vector3>.Add<Driver>(self).Finalize(to, duration);

    /// <summary>
    /// The driver is responsible for updating the tween's state.
    /// </summary>
    private class Driver : Tween<Vector3, Transform> {

      /// <summary>
      /// Overriden method which is called when the tween starts and should
      /// return the tween's initial value.
      /// </summary>
      public override Vector3 OnGetFrom() {
        return this.component.localPosition;
      }

      /// <summary>
      /// Overriden method which is called every tween update and should be used
      /// to update the tween's value.
      /// </summary>
      /// <param name="easedTime">The current eased time of the tween's step.</param>
      public override void OnUpdate(float easedTime) {
        this.valueCurrent.x = this.InterpolateValue(this.valueFrom.x, this.valueTo.x, easedTime);
        this.valueCurrent.y = this.InterpolateValue(this.valueFrom.y, this.valueTo.y, easedTime);
        this.valueCurrent.z = this.InterpolateValue(this.valueFrom.z, this.valueTo.z, easedTime);
        this.component.localPosition = this.valueCurrent;
      }
    }
  }
}