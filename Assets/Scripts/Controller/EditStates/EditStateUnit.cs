using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditStateUnit : EditStateDraw {
  protected override void OnNumber(object sender, object e) {
    units.PlaceEnemy(pos);
  }
  protected override void OnConfirm(object sender, object e) {
    units.SetSpawn(pos);
  }
  public override void Enter() {
    base.Enter();
    owner.textEditMode.text = "Units";
    owner.units.gameObject.SetActive(true);
    foreach (KeyValuePair<Point, Tile> tile in grid.tiles) {
      tile.Value.digitDisplayMode = DigitDisplayMode.Hidden;
    }
  }
}
