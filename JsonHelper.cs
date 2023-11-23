using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

public partial class JsonHelper 
{
  public static Dialog[] Parse(string npcName)
  {
    string projectPath = $"{ProjectSettings.GlobalizePath("res://")}/art/dialogs/{npcName}.json"; 
    string readText = File.ReadAllText(projectPath);
    Dialog[] dialogs = JsonSerializer.Deserialize<Dialog[]>(readText);

    return dialogs;
  }
}