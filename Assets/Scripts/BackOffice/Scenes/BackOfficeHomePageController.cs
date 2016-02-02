using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeHomePageController : BackOfficeController
{
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
		NewHomePageController.instance.cleanContents ();
		NewHomePageController.instance.cleanChallengeButtons ();
		NewHomePageController.instance.cleanFriendsStatusButtons ();
		NewHomePageController.instance.resize ();
		NewHomePageController.instance.initializeTabContent ();
	}
	public override void clickOnBackOfficeBackground()
	{
		NewHomePageController.instance.backOfficeBackgroundClicked ();
	}
	public override void moneyUpdate()
	{
		NewHomePageController.instance.moneyUpdate ();
	}
}

