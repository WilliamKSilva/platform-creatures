using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class JsonHelper 
{
  public void Parse()
  {
    string readText = File.ReadAllText("/home/williamkelvin/godot/projects/fight-your-demons/art/dialogs/grawg.json");
    Dialog[] jsonString = JsonSerializer.Deserialize<Dialog[]>(readText);
    GD.Print(jsonString[0].text);
  }
}