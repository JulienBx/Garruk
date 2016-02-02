using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeMarketController : BackOfficeController
{
	public override void sceneReturnPressed()
	{
		NewMarketController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewMarketController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewMarketController.instance.closeAllPopUp ();
	}
	public override void resizeAll()
	{
		base.resize ();
		NewMarketController.instance.cleanCards ();
		NewMarketController.instance.resize ();
		NewMarketController.instance.initializeCards ();
	}
	public override void clickOnBackOfficeBackground()
	{
		NewMarketController.instance.backOfficeBackgroundClicked ();
	}
	public override void moneyUpdate()
	{
		NewMarketController.instance.moneyUpdate ();
	}
}

