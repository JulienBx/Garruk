﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeProfileController : BackOfficeController
{
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
	public override void resizeAll()
	{
		base.resize ();
		NewProfileController.instance.cleanResultsContents ();
		NewProfileController.instance.cleanFriendsContents ();
		NewProfileController.instance.cleanChallengeButtons ();
		NewProfileController.instance.cleanFriendsStatusButtons ();
		NewProfileController.instance.resize ();
		NewProfileController.instance.initializeFriendsTabContent ();
		NewProfileController.instance.initializeResultsTabContent ();
	}
}

