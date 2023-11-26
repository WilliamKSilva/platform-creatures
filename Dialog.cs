using System;

public partial class Dialog
{
  public string text { get; set; }
  public string level { get; set; }
  public int order { get; set; }

  public string character { get; set; }

  public string displayed { get; set; }
}

public partial class PlayerDialogTypes
{
  public const string QUESTION = "QUESTION";
  public const string LAUGH = "LAUGH";

  public const string OK = "OK";
}
