using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class StoreMenuController : MenuController
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
	public override void moneyUpdate()
	{
		NewStoreController.instance.moneyUpdate ();
	}
	public override void initializeScene()
	{
		StartCoroutine (NewStoreController.instance.initialization ());
	}
}

