using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyMenuController : MenuController
{

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
	public override void initializeScene()
	{
		StartCoroutine (NewLobbyController.instance.initialization ());
	}
}

