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
	public override void resizeAll()
	{
		base.resize ();
		base.setCurrentPage (5);
		NewLobbyController.instance.resize ();
	}
	public override void initializeScene()
	{
		base.setCurrentPage (5);
		NewLobbyController.instance.endMenuInitialization ();
	}
}

