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
  public void Clamp(int min, int max) {
    Clamp(min, max, min, max);
  }
  public void Clamp(int Xmin, int Xmax, int yMin, int yMax) {
    if (x < Xmin) x = Xmin;
    if (x > Xmax) x = Xmax;
    if (y < yMin) y = yMin;
    if (y > yMax) y = yMax;
  }
}
