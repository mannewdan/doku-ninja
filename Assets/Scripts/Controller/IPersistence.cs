using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public interface IPersistence {
  public void SaveGridData(GridData data) {
    Debug.Log($"{data.id} {data.width} {data.height} {data.tiles.Count} {data.units.Count}");




  }
}
