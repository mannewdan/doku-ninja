using Tweens.Core;
using UnityEngine;

namespace Tweens {
  public static class SpriteRendererAlphaTween {
    public static Tween<float> TweenSpriteRendererAlpha(this Component self, float to, float duration) =>
      Tween<float>.Add<Driver>(self).Finalize(to, duration);

    public static Tween<float> TweenSpriteRendererAlpha(this GameObject self, float to, float duration) =>
      Tween<float>.Add<Driver>(self).Finalize(to, duration);

    /// <summary>
    /// The driver is responsible for updating the tween's state.
    /// </summary>
    private class Driver : Tween<float, SpriteRenderer> {
      private Color color;

      /// <summary>
      /// Overriden method which is called when the tween starts and should
      /// return the tween's initial value.
      /// </summary>
      public override float OnGetFrom() {
        return this.component.color.a;
      }

      /// <summary>
      /// Overriden method which is called every tween update and should be used
      /// to update the tween's value.
      /// </summary>
      /// <param name="easedTime">The current eased time of the tween's step.</param>
      public override void OnUpdate(float easedTime) {
        this.color = this.component.color;
        this.valueCurrent = this.InterpolateValue(this.valueFrom, this.valueTo, easedTime);
        this.color.a = this.valueCurrent;
        this.component.color = this.color;
      }
    }
  }
}