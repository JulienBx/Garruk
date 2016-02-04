using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeAuthenticationController : BackOfficeController
{
	public override void sceneReturnPressed()
	{
		AuthenticationController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		AuthenticationController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		//AuthenticationController.instance.closeAllPopUp ();
	}
	public override void resizeAll()
	{
		base.resize ();
		AuthenticationController.instance.resize ();
	}
}

