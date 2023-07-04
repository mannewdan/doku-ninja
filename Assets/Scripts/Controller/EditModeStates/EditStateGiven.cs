using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateGiven : EditState {
  protected override void OnMoveRepeat(object sender, object e) {
    pos += ((InfoEventArgs<Point>)e).info;
    pos = pos.Clamp(0, gridData.width - 1, 0, gridData.height - 1);
    SnapMarker();
  }
  protected override void OnNumber(object sender, object e) {
    var tile = grid.tiles.ContainsKey(pos) ? grid.tiles[pos] : null;
    if (tile) {
      tile.currentDigit = ((InfoEventArgs<int>)e).info;
    } else {
      Debug.Log("Couldn't find a tile at position: " + pos.ToString());
    }
  }
}
