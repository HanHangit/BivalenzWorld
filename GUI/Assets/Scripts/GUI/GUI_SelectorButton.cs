using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GUI.World;
using UnityEngine;
using UnityEngine.UI;

public class GUI_SelectorButton : MonoBehaviour
{
    [SerializeField]
    private GUI_TabNavigation.EType _type = default;
    [SerializeField]
    private Button _mainButton = default;
    [SerializeField]
    private Button _leftButton = default;
    [SerializeField]
    private Button _rightButton = default;
    [SerializeField]
    private GameObject _selectorPanel = default;

    [SerializeField]
    private GUI_Factory_Panel _leftPanelPrefab = default;
    [SerializeField]
    private GUI_Factory_PanelNavigation _leftPanelNavigationFactory = default;
    private PanelNavigation _leftPanelNavigation = null;
    [SerializeField]
    private GUI_TabNavigation _rightPanelNavigation = default;

    private void Awake()
    {
        _leftPanelNavigation = _leftPanelNavigationFactory.Create();
        ActivateSelector(false);
        _mainButton.onClick.AddListener(MainButtonPressedListener);
        _leftButton.onClick.AddListener(LeftButtonPressedListener);
        _rightButton.onClick.AddListener(RightButtonPressedListener);
    }

    private void RightButtonPressedListener()
    {
        _rightPanelNavigation.CreateObjFromType(_type);
        ActivateSelector(false);
    }

    private void LeftButtonPressedListener()
    {
        _leftPanelNavigation.AddAndShowPanel(_leftPanelPrefab.Create());
        ActivateSelector(false);
    }

    private void MainButtonPressedListener()
    {
        ActivateSelector(true);
    }

    private void ActivateSelector(bool active)
    {
        if (active && !_selectorPanel.activeInHierarchy)
        {
            _selectorPanel.gameObject.SetActive(true);
        }
        else
        {
            _selectorPanel.gameObject.SetActive(false);
        }
    }
}
