using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeStoreController : BackOfficeController
{
	public override void sceneReturnPressed()
	{
		NewStoreController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewStoreController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewStoreController.instance.closeAllPopUp ();
	}
	public override void resizeAll()
	{
		base.resize ();
		NewStoreController.instance.cleanPacks ();
		NewStoreController.instance.resize ();
		NewStoreController.instance.initializePacks ();
	}
	public override void moneyUpdate()
	{
		NewStoreController.instance.moneyUpdate ();
	}
	public override void clickOnBackOfficeBackground()
	{
		NewStoreController.instance.backOfficeBackgroundClicked ();
	}
}

