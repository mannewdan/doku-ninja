using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U {
  public static List<T> Shuffle<T>(List<T> list) {
    List<T> oldList = list != null ? new List<T>(list) : new List<T>();
    List<T> newList = new List<T>();
    int i;
    while (oldList.Count > 0) {
      i = Random.Range(0, oldList.Count);
      newList.Add(oldList[i]);
      oldList.RemoveAt(i);
    }

    return newList;
  }
}
