using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpResultLobbyController : PopUpController
{
	
	public override void startHoveringPopUp()
	{
		NewLobbyController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewLobbyController.instance.endHoveringPopUp();
	}
	public void show(PlayerResult pR)
	{
		//gameObject.transform.FindChild ("date").GetComponent<TextMeshPro> ().text = n.Notification.Date.ToString ();
		//gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = n.Content;
		//gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (n.SendingUser);
	}
}

