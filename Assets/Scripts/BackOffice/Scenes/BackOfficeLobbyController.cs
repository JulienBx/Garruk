using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyBackOfficeController : BackOfficeController
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
		NewLobbyController.instance.cleanLastResults ();
		NewLobbyController.instance.resize ();
		NewLobbyController.instance.initializeResults ();
	}
}

