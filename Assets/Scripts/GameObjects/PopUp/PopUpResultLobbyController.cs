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
		gameObject.transform.FindChild ("user").GetComponent<PopUpUserController> ().show (pR.Opponent);
		gameObject.transform.FindChild ("date").GetComponent<TextMeshPro> ().text = pR.Date.ToString ();
		if(pR.HasWon)
		{
			//tempColor = new Color(75f/255f,163f/255f,174f/255f);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "Victoire";
			
		}
		else
		{
			//tempColor = new Color(233f/255f,140f/255f,140f/255f);
			gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = "Défaite";
		}
		//gameObject.transform.FindChild ("content").GetComponent<TextMeshPro> ().text = n.Content;

	}
}

