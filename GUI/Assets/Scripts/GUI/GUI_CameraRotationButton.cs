using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CameraRotationButton : MonoBehaviour
{
    [SerializeField]
    private Button _rotationButton = default;
    [SerializeField]
    private TMPro.TextMeshProUGUI _defaultText = default;
    private bool _is3DMode = true;

    private void Start()
    {
        _defaultText.text = "2D";
        _is3DMode = true;
        _rotationButton.onClick.AddListener(ButtonClickedListener);
    }

    private void ButtonClickedListener()
    {
        _is3DMode = !_is3DMode;
        if (_is3DMode)
        {
            SetText("2D");
        }
        else
        {
            SetText("3D");
        }
    }

    private void SetText(string txt)
    {
        _defaultText.text = txt;
    }
}
