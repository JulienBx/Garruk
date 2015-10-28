using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class HomePageMenuController : MenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewHomePageController.instance.initialization ());
		base.Start ();
	}
	public override void sceneReturnPressed()
	{
		NewHomePageController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewHomePageController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewHomePageController.instance.closeAllPopUp ();
	}
	public override void resizeAll()
	{
		base.resize ();
		NewHomePageController.instance.resize ();
	}
}

