using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point {
  public int x;
  public int y;

  public Point(int x, int y) {
    this.x = x;
    this.y = y;
  }

  public static Point operator +(Point a, Point b) {
    return new Point(a.x + b.x, a.y + b.y);
  }
  public static Point operator -(Point p1, Point p2) {
    return new Point(p1.x - p2.x, p1.y - p2.y);
  }
  public static bool operator ==(Point a, Point b) {
    return a.x == b.x && a.y == b.y;
  }
  public static bool operator !=(Point a, Point b) {
    return !(a == b);
  }
  public override bool Equals(object obj) {
    if (!(obj is Point)) return false;
    Point p = (Point)obj;
    return x == p.x && y == p.y;
  }
  public bool Equals(Point p) {
    return x == p.x && y == p.y;
  }
  public override int GetHashCode() {
    return x ^ y;
  }
  public override string ToString() {
    return string.Format("({0}, {1})", x, y);
  }
  public Point Clamp(int min, int max) {
    return Clamp(min, max, min, max);
  }
  public Point Clamp(int Xmin, int Xmax, int yMin, int yMax) {
    Point p = this;
    if (p.x < Xmin) p.x = Xmin;
    if (p.x > Xmax) p.x = Xmax;
    if (p.y < yMin) p.y = yMin;
    if (p.y > yMax) p.y = yMax;
    return p;
  }
  public Point Normalized(bool oneAxisOnly = false) {
    Point p = this;
    if (p.x > 1) p.x = 1;
    if (p.x < -1) p.x = -1;
    if (p.y > 1) p.y = 1;
    if (p.y < -1) p.y = 1;

    if (oneAxisOnly && p.x != 0 && p.y != 0) p.y = 0;
    return p;
  }
  public float Dist(Point other, bool allowDiagonals = false) {
    var xDist = Mathf.Abs(other.x - x);
    var yDist = Mathf.Abs(other.y - y);
    if (allowDiagonals) {
      var diff = Mathf.Abs(xDist - yDist);
      var min = Mathf.Min(xDist, yDist);
      return min * 1.4f + diff;
    } else return xDist + yDist;
  }
}
