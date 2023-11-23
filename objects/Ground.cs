using Godot;
using System;

public partial class Ground : StaticBody2D
{
  public override void _Ready()
  {
    Polygon2D polygon = GetNode<Polygon2D>("Polygon2D");
    var polygonShape = polygon.Polygon;
    polygon.Polygon = polygonShape;

    CollisionPolygon2D polygonCollision = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
    polygonCollision.Polygon = polygon.Polygon;
  }
}
