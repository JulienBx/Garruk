using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MarketMenuController : MenuController
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
	public override void moneyUpdate()
	{
		NewMarketController.instance.moneyUpdate ();
	}
	public override void initializeScene ()
	{
		StartCoroutine (NewMarketController.instance.initialization ());
	}
}

