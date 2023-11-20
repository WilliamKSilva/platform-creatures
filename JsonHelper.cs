using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

public partial class JsonHelper 
{
  public static void Parse(string npcName)
  {
    string projectPath = $"{ProjectSettings.GlobalizePath("res://")}/art/dialogs/{npcName}.json"; 
    string readText = File.ReadAllText(projectPath);
    Dialog[] jsonString = JsonSerializer.Deserialize<Dialog[]>(readText);
    GD.Print(jsonString[0].text);
  }
}