using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpFriendController : PopUpController
{

	public void show(User u)
	{
		if(u.OnlineStatus==0)
		{
			gameObject.transform.FindChild("Button").gameObject.SetActive(false);
			gameObject.transform.FindChild("Button2").gameObject.SetActive(false);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "n'est pas en ligne";
		}
		else if(u.OnlineStatus==1)
		{
			gameObject.transform.FindChild("Button").gameObject.SetActive(true);
			gameObject.transform.FindChild("Button2").gameObject.SetActive(false);
			gameObject.transform.FindChild ("Button").transform.localPosition = new Vector3 (0f, -0.6f, -1f);
			gameObject.transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text="Défier";
			this.setButtonController();
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "est en ligne";
		}
		else if(u.OnlineStatus==2)
		{
			gameObject.transform.FindChild("Button").gameObject.SetActive(false);
			gameObject.transform.FindChild("Button2").gameObject.SetActive(false);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "a rejoint un match";
		}
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (u);
	}
	public void show2(User u)
	{
		gameObject.transform.FindChild("Button").gameObject.SetActive(false);
		gameObject.transform.FindChild("Button2").gameObject.SetActive(false);
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (u);
	}
	public virtual void setButtonController()
	{
	}
}

