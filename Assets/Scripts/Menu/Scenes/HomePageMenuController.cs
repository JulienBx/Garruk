using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class HomePageMenuController : MenuController
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
		base.refreshMenuObject ();
		base.setCurrentPage (0);
		NewHomePageController.instance.cleanContents ();
		NewHomePageController.instance.cleanChallengeButtons ();
		NewHomePageController.instance.cleanFriendsStatusButtons ();
		NewHomePageController.instance.resize ();
		NewHomePageController.instance.initializeTabContent ();
	}
	public override void moneyUpdate()
	{
		NewHomePageController.instance.moneyUpdate ();
	}
	public override void initializeScene()
	{
		base.setCurrentPage (0);
		NewHomePageController.instance.endMenuInitialization ();
	}
	public override void clickOnBackOfficeBackground()
	{
		NewHomePageController.instance.backOfficeBackgroundClicked ();
	}
}

