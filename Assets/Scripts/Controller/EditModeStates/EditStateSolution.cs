using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateSolution : EditStateDraw {
  protected override void OnCancel(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.solutionDigit = 0;
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
      if (tile) {
        tile.solutionDigit = number;
      } else {
        Debug.Log("Couldn't find a tile at position: " + pos.ToString());
      }
    }
  }
  public override void Enter() {
    base.Enter();
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.SetDisplayMode(DigitDisplayMode.Solution);
    }
  }
  public override void Exit() {
    base.Exit();
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.SetDisplayMode(DigitDisplayMode.Current);
    }
  }
}
