using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ProfileMenuController : MenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewProfileController.instance.initialization ());
		base.Start ();
	}
	public override void sceneReturnPressed()
	{
		NewProfileController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewProfileController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewProfileController.instance.closeAllPopUp ();
	}
	
}

