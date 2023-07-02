using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TilemapReader : MonoBehaviour {
  [SerializeField] private AssetLabelReference assetLabelReference;
  [SerializeField] private List<string> DEBUG_tilemapIds;
  [SerializeField] private Dictionary<string, List<string>> tilemapFiles = new Dictionary<string, List<string>>();

  void Awake() {
    Addressables.LoadAssetsAsync<TextAsset>(assetLabelReference, (file) => {
      string[] segments = file.name.Split('_');
      if (segments.Length < 2) return;

      string id = segments[1];
      if (!tilemapFiles.ContainsKey(id)) tilemapFiles.Add(id, new List<string>());
      tilemapFiles[id].Add(file.name);
    }).Completed += (t) => {
      Addressables.Release(t);
      DEBUG_tilemapIds = new List<string>(tilemapFiles.Keys);
    };
  }

  public GridData LoadTilemap(string id) {
    if (!tilemapFiles.ContainsKey(id)) {
      Debug.LogError("Tilemap ID not found: " + id);
      return null;
    }

    GridData data = new GridData();
    data.id = id;
    List<string> filenames = tilemapFiles[id];
    foreach (string filename in filenames) {
      Addressables.LoadAssetAsync<TextAsset>(filename).Completed += (asset) => {
        Debug.Log(asset);
      };
    }

    return data;
  }
}
