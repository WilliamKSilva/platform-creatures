using Godot;
using System;

public partial class Roof : StaticBody2D
{
  public override void _Ready()
  {
    Polygon2D polygon2D = GetNode<Polygon2D>("Polygon2D");
    var polygonShape = polygon2D.Polygon;
    polygon2D.Polygon = polygonShape;

    CollisionPolygon2D polygon2DCollision = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
    polygon2DCollision.Polygon = polygon2D.Polygon;
  }
}
