using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegraphs : MonoBehaviour {
  [SerializeField] GameObject telegraphPrefab;
  Dictionary<Point, List<GameObject>> telegraphs = new Dictionary<Point, List<GameObject>>();

  public void Add(Point pos) {
    if (!telegraphs.ContainsKey(pos)) {
      telegraphs.Add(pos, new List<GameObject>());
    }

    bool extraFound = false;
    for (int i = 0; i < telegraphs[pos].Count; i++) {
      if (!telegraphs[pos][i].activeSelf) {
        telegraphs[pos][i].SetActive(true);
        extraFound = true;
        break;
      }
    }

    if (!extraFound) {
      GameObject newVisual = Instantiate(telegraphPrefab);
      newVisual.transform.SetParent(transform);
      newVisual.transform.localPosition = new Vector3(pos.x, pos.y);
      telegraphs[pos].Add(newVisual);
    }
  }
  public void Remove(Point pos) {
    if (!telegraphs.ContainsKey(pos)) return;

    for (int i = 0; i < telegraphs[pos].Count; i++) {
      if (telegraphs[pos][i].activeSelf) {
        telegraphs[pos][i].SetActive(false);
        break;
      }
    }
  }
}
