using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateSolution : EditStateDraw {
  protected override void OnConfirm(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.solutionDigit = 0;
      tile.Evaluate();
      tile.digitDisplayMode = DigitDisplayMode.EditSolution;
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
      if (tile) {
        tile.solutionDigit = number;
        tile.Evaluate();
        tile.digitDisplayMode = DigitDisplayMode.EditSolution;
      } else {
        Debug.Log("Couldn't find a tile at position: " + pos.ToString());
      }
    }
  }
  public override void Enter() {
    base.Enter();
    owner.textEditMode.text = "Solution";
    owner.units.gameObject.SetActive(false);
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.digitDisplayMode = DigitDisplayMode.EditSolution;
    }
  }
}
