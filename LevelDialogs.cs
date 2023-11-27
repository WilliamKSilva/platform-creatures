using Godot;
using System;

public class LevelDialogs
{
  public Dialog[] dialogs = Array.Empty<Dialog>();
  public int currentOrder = 0;

  public Dialog GetCurrent()
  {
    for (int i = 0; i < dialogs.Length; i++)
    {
      Dialog dialog = dialogs[i];

      if (dialog == null)
      {
        return null;
      }

      if (dialog.order == currentOrder)
      {
        return dialog;
      }
    }

    return null;
  }

  public Dialog GetLast()
  {
    Dialog lastDialog = dialogs[0];

    for (int i = 0; i < dialogs.Length; i++)
    {
      Dialog currentDialog = dialogs[i];

      if (currentDialog.order > lastDialog.order)
      {
        lastDialog = currentDialog;
        continue;
      }
    }

    return lastDialog;
  }

  public bool IsEnded()
  {
    bool lastDialogDisplayed = GetLast().order == currentOrder;

    return lastDialogDisplayed;
  }
}
