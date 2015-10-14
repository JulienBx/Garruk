using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpChallengesRecordProfileController : PopUpController
{
	
	public override void startHoveringPopUp()
	{
		NewProfileController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewProfileController.instance.endHoveringPopUp();
	}
	public void show(ChallengesRecord c)
	{
		gameObject.transform.FindChild ("date").gameObject.SetActive (false);
		if(c.NbWins>c.NbLooses)
		{
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "Votre ami est actuellement le plus fort !";
		}
		else if(c.NbLooses > c.NbWins)
		{
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "Vous êtes le plus fort !";
		}
		else
		{
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "Ex eaquo ! pour le moment...";
		}
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (c.Friend);
	}
}

