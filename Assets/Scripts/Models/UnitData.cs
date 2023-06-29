using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : ScriptableObject {
  public readonly bool isPlayer;
  public Point pos;

  public UnitData(bool isPlayer) {
    this.isPlayer = isPlayer;
  }
}
