using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpFriendHomePageController : PopUpController
{
	
	public override void startHoveringPopUp()
	{
		NewHomePageController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewHomePageController.instance.endHoveringPopUp();
	}
	public void show(User u)
	{
		if(u.OnlineStatus==0)
		{
			gameObject.transform.FindChild("invitationButton").gameObject.SetActive(false);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "n'est pas en ligne";
		}
		else if(u.OnlineStatus==1)
		{
			gameObject.transform.FindChild("invitationButton").gameObject.SetActive(true);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "est en ligne";
		}
		else if(u.OnlineStatus==2)
		{
			gameObject.transform.FindChild("invitationButton").gameObject.SetActive(false);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "a rejoint un match";
		}
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (u);
	}
}

