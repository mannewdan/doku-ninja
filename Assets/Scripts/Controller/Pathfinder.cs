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
  public readonly bool initialized;

  [SerializeField] private List<Node> openList = new List<Node>();
  [SerializeField] private List<Node> closedList = new List<Node>();
  [SerializeField] private List<Node> fullPath = new List<Node>();
  [SerializeField] private List<Node> noclipPath = new List<Node>();
  [SerializeField] private List<Node> bestPath = new List<Node>();

  public Pathfinder(Grid grid, UnitManager units) {
    this.grid = grid;
    this.units = units;
    initialized = true;
  }

  public void FindPath(Point start, Point end) {
    fullPath.Clear();
    noclipPath.Clear();
    bestPath.Clear();

    fullPath = FindPath(start, end, true);
    noclipPath = FindPath(start, end, false);

    if (fullPath.Count - noclipPath.Count >= 2) {
      bestPath = noclipPath;
    } else {
      bestPath = fullPath;
    }
  }
  public List<Node> FindPath(Point start, Point end, bool avoidUnits) {
    openList.Clear();
    closedList.Clear();
    List<Node> newPath = new List<Node>();

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
          newPath.Insert(0, current.parent);
          current = current.parent;
        }
        while (newPath.Count > 0 && newPath[0].IsEqual(startNode)) {
          newPath.RemoveAt(0);
        }
        return newPath;
      }

      openList.Remove(current);
      closedList.Add(current);

      //add neighbors
      Point n = new Point(current.point.x, current.point.y + 1);
      if (grid.InBounds(n) &&
      (!avoidUnits || (avoidUnits && !units.IsOccupied(n))) &&
      !openList.Exists(node => node.point == n) &&
      !closedList.Exists(node => node.point == n)) {
        openList.Add(new Node(current, 0, 1, endNode));
      }
      Point s = new Point(current.point.x, current.point.y - 1);
      if (grid.InBounds(s) &&
      (!avoidUnits || (avoidUnits && !units.IsOccupied(s))) &&
      !openList.Exists(node => node.point == s) &&
      !closedList.Exists(node => node.point == s)) {
        openList.Add(new Node(current, 0, -1, endNode));
      }
      Point e = new Point(current.point.x + 1, current.point.y);
      if (grid.InBounds(e) &&
      (!avoidUnits || (avoidUnits && !units.IsOccupied(e))) &&
      !openList.Exists(node => node.point == e) &&
      !closedList.Exists(node => node.point == e)) {
        openList.Add(new Node(current, 1, 0, endNode));
      }
      Point w = new Point(current.point.x - 1, current.point.y);
      if (grid.InBounds(w) &&
      (!avoidUnits || (avoidUnits && !units.IsOccupied(w))) &&
      !openList.Exists(node => node.point == w) &&
      !closedList.Exists(node => node.point == w)) {
        openList.Add(new Node(current, -1, 0, endNode));
      }

      failsafe++;
    } while (openList.Count > 0 && failsafe < 10000);

    return newPath;
  }

  public bool NextStep(out Point p) {
    p = new Point(0, 0);
    if (bestPath.Count > 0) {
      p = bestPath[0].point;
      bestPath.RemoveAt(0);
      return true;
    } else return false;
  }
  public Point SmartStep(Point start, Point end) {
    Point p = start;
    var dist = start.Dist(end);

    List<Point> closerPoints = new List<Point>();
    List<Point> fartherPoints = new List<Point>();
    List<Point> pointsToConsider = new List<Point>() {
      new Point(start.x, start.y + 1),
      new Point(start.x, start.y - 1),
      new Point(start.x + 1, start.y),
      new Point(start.x - 1, start.y) };

    foreach (Point a in pointsToConsider) {
      if (!grid.InBounds(a) || units.IsOccupied(a)) continue;

      if (a.Dist(end) < dist) {
        closerPoints.Add(a);
      } else {
        fartherPoints.Add(a);
      }
    }

    if (closerPoints.Count > 0) {
      return closerPoints[Random.Range(0, closerPoints.Count)];
    } else if (fartherPoints.Count > 0 && Random.Range(0f, 1f) < 0.25f) {
      return fartherPoints[Random.Range(0, fartherPoints.Count)];
    }

    return p;
  }
}
