using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpAcceptFriendsRequestButtonController : PopUpButtonController
{
	private bool isHovering;
	
	public override void clickHandler()
	{
		NewProfileController.instance.acceptFriendsRequestHandler ();
	}
}
