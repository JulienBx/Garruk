using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class StoreMenuController : MenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewStoreController.instance.initialization ());
		base.Start ();
	}
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
}

