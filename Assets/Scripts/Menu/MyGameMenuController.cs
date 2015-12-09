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
		base.setCurrentPage (1);
		newMyGameController.instance.cleanCards ();
		newMyGameController.instance.resize ();
		newMyGameController.instance.initializeCards ();
	}
	public override void moneyUpdate()
	{
		newMyGameController.instance.moneyUpdate ();
	}
	public override void initializeScene()
	{
		base.setCurrentPage (1);
		newMyGameController.instance.endMenuInitialization ();
	}
	public override void clickOnBackOfficeBackground()
	{
		newMyGameController.instance.backOfficeBackgroundClicked ();
	}
}

