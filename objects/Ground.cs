using Godot;
using System;

public partial class Ground : StaticBody2D
{
  [Export]
  public int PolygonXWidth { get; set; } 

  public override void _Ready()
  {
    Polygon2D polygon = GetNode<Polygon2D>("Polygon2D");
    var polygonShape = polygon.Polygon;

    polygonShape[2].X = PolygonXWidth;
    polygonShape[3].X = PolygonXWidth;
    polygonShape[4].X = PolygonXWidth;

    polygon.Polygon = polygonShape;

    CollisionPolygon2D polygonCollision = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
    polygonCollision.Polygon = polygon.Polygon;
  }
}
