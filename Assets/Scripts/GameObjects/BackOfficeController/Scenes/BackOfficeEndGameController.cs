using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeEndGameController : BackOfficeController
{
	public override void sceneReturnPressed()
	{
		NewEndGameController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewEndGameController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		
	}
	public override void resizeAll()
	{
		base.resize ();
		NewEndGameController.instance.resize ();
	}
	public override int getLoadingScreenBackground()
	{
		return 1;
	}
}

