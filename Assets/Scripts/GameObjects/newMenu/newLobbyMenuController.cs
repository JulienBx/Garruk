using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newLobbyMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewLobbyController.instance.initialization ());
		base.Start ();
	}
	public override void sceneReturnPressed()
	{
		NewLobbyController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewLobbyController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewLobbyController.instance.closeAllPopUp ();
	}
}

