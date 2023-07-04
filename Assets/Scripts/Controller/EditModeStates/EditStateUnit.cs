using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateUnit : EditStateDraw {
  protected override void OnNumber(object sender, object e) {

  }
  public override void Enter() {
    base.Enter();
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.SetDisplayMode(DigitDisplayMode.Faded);
    }
  }
  public override void Exit() {
    base.Exit();
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.SetDisplayMode(DigitDisplayMode.Current);
    }
  }
}
