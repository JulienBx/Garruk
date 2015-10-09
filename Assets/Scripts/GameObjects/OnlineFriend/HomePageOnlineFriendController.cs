using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class HomePageOnlineFriendController : OnlineFriendController
{

	public override void startHovering()
	{
		NewHomePageController.instance.startHoveringFriend (base.Id);
	}
	public override void endHovering()
	{
		NewHomePageController.instance.endHoveringFriend ();
	}
}

