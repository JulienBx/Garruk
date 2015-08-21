using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpNotificationHomePageController : PopUpController
{

	public override void startHoveringPopUp()
	{
		NewHomePageController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewHomePageController.instance.endHoveringPopUp();
	}
	public void show(DisplayedNotification n)
	{
		gameObject.transform.FindChild ("date").GetComponent<TextMeshPro> ().text = n.Notification.Date.ToString ();
		gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = n.Content;
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (n.SendingUser);
	}
}
