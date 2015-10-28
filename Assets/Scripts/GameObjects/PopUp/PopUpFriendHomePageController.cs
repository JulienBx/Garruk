using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpFriendHomePageController : PopUpFriendController
{
	
	public override void startHoveringPopUp()
	{
		//NewHomePageController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		//NewHomePageController.instance.endHoveringPopUp();
	}
	public override void setButtonController()
	{
		gameObject.transform.FindChild ("Button").gameObject.AddComponent <PopUpFriendButtonHomePageController>();
	}
}

