using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MyGameMenuController : MenuController
{
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
	public override void resizeAll()
	{
		base.resize ();
		newMyGameController.instance.resize ();
	}
	public override void moneyUpdate()
	{
		newMyGameController.instance.moneyUpdate ();
	}
	public override void initializeScene()
	{
		base.setCurrentPage (1);
		StartCoroutine (newMyGameController.instance.initialization ());
	}
}
