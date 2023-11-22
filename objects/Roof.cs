using Godot;
using System;

public partial class Roof : StaticBody2D
{
  [Export]
  public int PolygonXWidth { get; set; }

  public override void _Ready()
  {
    Polygon2D polygon2D = GetNode<Polygon2D>("Polygon2D");
    var polygonShape = polygon2D.Polygon;

    GD.Print(polygon2D.Polygon[2]);

    polygonShape[26].X = PolygonXWidth;
    polygonShape[27].X = PolygonXWidth + 300;
    polygonShape[28].X = PolygonXWidth + 500;

    polygon2D.Polygon = polygonShape;

    CollisionPolygon2D polygon2DCollision = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
    polygon2DCollision.Polygon = polygon2D.Polygon;
  }
}
