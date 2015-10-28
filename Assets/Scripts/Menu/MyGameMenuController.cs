using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MyGameMenuController : MenuController
{
	
	public override void Start ()
	{
		StartCoroutine (newMyGameController.instance.initialization ());
		base.Start ();
	}
	public override void sceneReturnPressed()
	{
		newMyGameController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		newMyGameController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		newMyGameController.instance.closeAllPopUp ();
	}
	
}

