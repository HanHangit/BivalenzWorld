﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Navigation_Text : GUI_TabNavigation
{
	[SerializeField]
	private Button _createNewTextButton = default;

	[SerializeField]
	protected GUI_TextFieldCloseButton _textfieldButtonPrefab = default;
	[SerializeField]
	protected GUI_TextFieldButton _textFieldButtonDefault = default;
	[SerializeField]
	protected APage _page = default;
	[SerializeField]
	private GUI_TextInputField _field = default;

	private GUI_TextFieldButton _currentSelectedButton = default;

	public GUI_TextFieldButton GetCurrentSelectedButton()
	{
		return _currentSelectedButton;
    }

    protected override void OnAwake()
	{
		base.OnAwake();
		
		_textPanelsInstances.Add(new Pair()
		{
				Button = _textFieldButtonDefault,
				Page = _page
		});

		_textFieldButtonDefault.SetTexts(new List<string>());
		SetCurrentSelectButton(_textFieldButtonDefault);
		_textFieldButtonDefault.GetButton().onClick.AddListener(() => ButtonClickedListener(_textFieldButtonDefault));
		ButtonClickedListener(_textFieldButtonDefault);
		_createNewTextButton.onClick.AddListener(CreateNewTextButtonListener);
	}

	public void CreateTextInstance(string name, List<string> text)
	{
		GUI_TextFieldCloseButton instance = Instantiate(_textfieldButtonPrefab, _buttonAnchor);
		var page = new Pair()
		{
				Button = instance,
				Page = _page
		};

		instance.SetButtonName(name);
		_textPanelsInstances.Add(page);
		instance.SetTexts(text);
		SetCurrentSelectButton(instance);
		instance.GetButton().onClick.AddListener(() => ButtonClickedListener(instance));
		instance.DestroyObjectEvent.AddEventListener(RemoveTextInstance);
		ButtonClickedListener(instance);
	}

	public void OverwriteCurrentText(string name, List<string> text)
	{
		_currentSelectedButton.SetButtonName(name);
		_currentSelectedButton.SetTexts(text);
		SetCurrentInstanceText(_currentSelectedButton);
	}

	private void SetCurrentSelectButton(GUI_TextFieldButton newFieldButton)
	{
		if (_currentSelectedButton != null && newFieldButton != _currentSelectedButton)
		{
			_currentSelectedButton.UnHover();
		}

		_currentSelectedButton = newFieldButton;
		_currentSelectedButton.Hover();
	}

	private void RemoveTextInstance(GUI_TextFieldButton arg0)
	{
		var instance = _textPanelsInstances.Find(x => x.Button == arg0);
		if (instance != null)
		{
			_textPanelsInstances.Remove(instance);
			Destroy(instance.Button.gameObject);
		}

		var fieldButton = _textPanelsInstances[0];
		ButtonClickedListener(fieldButton.Button);
	}

	private void CreateNewTextButtonListener()
	{
		CreateTextInstance("Untitled Sentences",new List<string>()
		{
				""
		});
	}

	private void ButtonClickedListener(GUI_TabButton button)
	{
		SaveText(_currentPageInstance.Button as GUI_TextFieldButton, _field);
		ActivatePanel(GUI_TabNavigation.EType.Sentences);
		SetCurrentInstanceText(button as GUI_TextFieldButton);
	}

	private void SaveText(GUI_TextFieldButton button, GUI_TextInputField field)
	{
		if (button != null)
		{
			button.SetTexts(field.GetInputFieldText());
		}
	}

	private void SetCurrentInstanceText(GUI_TextFieldButton textfield)
	{
		SetCurrentSelectButton(textfield);
		_currentPageInstance.Button = textfield;
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
			List<string> text = textfield.GetText();
			CleanField(field);

			if (text.Count > 0)
			{
				field.AddNewTextToDefault(text[0]);
			}

			if (text.Count > 1)
			{
				for (int i = 1; i < text.Count; i++)
				{
					field.AddNewSentence(text[i]);
				}
			}
		}
	}

	private void CleanField(GUI_TextInputField field)
	{
		field.DeleteSentencesTexts();
	}

	public override void CreateGame()
	{
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentPageInstance.Button as GUI_TextFieldButton, field);
		}

		base.CreateGame();
	}

	public override void CreateModelRepresentation()
	{
		if (_currentPageInstance.Page is GUI_TextInputField field)
		{
			SaveText(_currentPageInstance.Button as GUI_TextFieldButton, field);
		}
		base.CreateModelRepresentation();
	}
}
