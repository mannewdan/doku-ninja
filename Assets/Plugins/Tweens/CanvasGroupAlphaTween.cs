using Tweens.Core;
using UnityEngine;

namespace Tweens {
  public static class CanvasGroupAlphaTween {
    public static Tween<float> TweenCanvasGroupAlpha(this Component self, float to, float duration) =>
      Tween<float>.Add<Driver>(self).Finalize(to, duration);

    public static Tween<float> TweenCanvasGroupAlpha(this GameObject self, float to, float duration) =>
      Tween<float>.Add<Driver>(self).Finalize(to, duration);

    /// <summary>
    /// The driver is responsible for updating the tween's state.
    /// </summary>
    private class Driver : Tween<float, CanvasGroup> {

      /// <summary>
      /// Overriden method which is called when the tween starts and should
      /// return the tween's initial value.
      /// </summary>
      public override float OnGetFrom() {
        return this.component.alpha;
      }

      /// <summary>
      /// Overriden method which is called every tween update and should be used
      /// to update the tween's value.
      /// </summary>
      /// <param name="easedTime">The current eased time of the tween's step.</param>
      public override void OnUpdate(float easedTime) {
        this.valueCurrent = this.InterpolateValue(this.valueFrom, this.valueTo, easedTime);
        this.component.alpha = this.valueCurrent;
      }
    }
  }
}