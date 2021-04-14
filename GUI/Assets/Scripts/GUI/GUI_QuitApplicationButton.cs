using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_QuitApplicationButton : GUI_Button
{
	protected override void ButtonClickedListener()
	{
		Application.Quit();
	}
}
