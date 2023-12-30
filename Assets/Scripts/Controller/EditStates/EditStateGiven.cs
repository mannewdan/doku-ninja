using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateGiven : EditStateDraw {
  protected override void OnConfirm(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.currentDigit = 0;
      tile.digitDisplayMode = DigitDisplayMode.EditGiven;
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }
  protected override void OnNumber(object sender, object e) {
    if (e is int number) {
      var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
      if (tile) {
        tile.currentDigit = 0;
        tile.currentDigit = number;
        tile.digitDisplayMode = DigitDisplayMode.EditGiven;
      } else {
        Debug.Log("Couldn't find a tile at position: " + pos.ToString());
      }
    }
  }
  public override void Enter() {
    base.Enter();
    owner.textEditMode.text = "Givens";
    owner.units.gameObject.SetActive(true);
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.digitDisplayMode = DigitDisplayMode.EditGiven;
    }
  }
}
