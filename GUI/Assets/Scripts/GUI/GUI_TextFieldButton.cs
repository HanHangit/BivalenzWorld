using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_TextFieldButton : GUI_TabButton
{
    private List<string> _texts = new List<string>();
    private string _savePath = "";
    public string SavePath => _savePath;
    private bool _hasUnsavedChanges = false;

    public void SetSavePath(string path)
    {
        _savePath = path;
    }

    public void SetTexts(List<string> txt)
    {
        _hasUnsavedChanges = true;
        _texts = txt;
    }

    public List<string> GetText()
    {
        return _texts;
    }
}
