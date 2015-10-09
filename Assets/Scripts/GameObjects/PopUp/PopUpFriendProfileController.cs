using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpFriendProfileController : PopUpFriendController
{
	
	public override void startHoveringPopUp()
	{
		NewProfileController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewProfileController.instance.endHoveringPopUp();
	}
}

