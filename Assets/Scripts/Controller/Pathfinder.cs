using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pathfinder {
  [System.Serializable]
  public class Node {
    public Node(Point p) {
      point = p;
    }
    public Node(Node prev, int xDiff, int yDiff, Node end) {
      point = prev.point + new Point(xDiff, yDiff);
      f = prev.f + Mathf.Abs(xDiff) + Mathf.Abs(yDiff);
      g = Mathf.Abs((this.point - end.point).x) + Mathf.Abs((this.point - end.point).y);
      h = f + g;
      parent = prev;
    }

    public Point point;

    public int f; //how far from start
    public int g; //how far from end
    public int h; //f + g

    public Node parent; //for tracing the path back to start

    public bool IsEqual(Node other) {
      return point == other.point;
    }
    public bool IsAdjacent(Node other) {
      return Mathf.Abs(point.x - other.point.x) + Mathf.Abs(point.y - other.point.y) <= 1;
    }
  }

  private Grid grid;
  private UnitManager units;

  [SerializeField] private List<Node> openList = new List<Node>();
  [SerializeField] private List<Node> closedList = new List<Node>();
  [SerializeField] private List<Node> bestPath = new List<Node>();

  public Pathfinder(Grid grid, UnitManager units) {
    this.grid = grid;
    this.units = units;
  }

  public void FindPath(Point start, Point end) {
    openList.Clear();
    closedList.Clear();
    bestPath.Clear();

    Node startNode = new Node(start);
    Node endNode = new Node(end);
    openList.Add(startNode);
    endNode.parent = startNode;

    int failsafe = 0;
    do {
      //find lowest h
      Node current = openList[0];
      foreach (Node node in openList) {
        if (node.h < current.h) {
          current = node;
        } else if (node.h == current.h && (node.g < current.g || Random.Range(0.0f, 1.0f) < 0.5f)) {
          current = node;
        }
      }

      //path found
      if (current.IsAdjacent(endNode)) {
        endNode.parent = current;
        current = endNode;
        while (current.parent != null) {
          bestPath.Insert(0, current.parent);
          current = current.parent;
        }
        while (bestPath.Count > 0 && bestPath[0].IsEqual(startNode)) {
          bestPath.RemoveAt(0);
        }
        return;
      }

      openList.Remove(current);
      closedList.Add(current);

      //add neighbors
      Point n = new Point(current.point.x, current.point.y + 1);
      if (grid.InBounds(n) &&
      !units.IsOccupied(n) &&
      !openList.Exists(node => node.point == n) &&
      !closedList.Exists(node => node.point == n)) {
        openList.Add(new Node(current, 0, 1, endNode));
      }
      Point s = new Point(current.point.x, current.point.y - 1);
      if (grid.InBounds(s) &&
      !units.IsOccupied(s) &&
      !openList.Exists(node => node.point == s) &&
      !closedList.Exists(node => node.point == s)) {
        openList.Add(new Node(current, 0, -1, endNode));
      }
      Point e = new Point(current.point.x + 1, current.point.y);
      if (grid.InBounds(e) &&
      !units.IsOccupied(e) &&
      !openList.Exists(node => node.point == e) &&
      !closedList.Exists(node => node.point == e)) {
        openList.Add(new Node(current, 1, 0, endNode));
      }
      Point w = new Point(current.point.x - 1, current.point.y);
      if (grid.InBounds(w) &&
      !units.IsOccupied(w) &&
      !openList.Exists(node => node.point == w) &&
      !closedList.Exists(node => node.point == w)) {
        openList.Add(new Node(current, -1, 0, endNode));
      }

      failsafe++;
    } while (openList.Count > 0 && failsafe < 10000);
  }
  public bool NextStep(out Point p) {
    p = new Point(0, 0);
    if (bestPath.Count > 0) {
      p = bestPath[0].point;
      bestPath.RemoveAt(0);
      return true;
    } else return false;
  }
}
