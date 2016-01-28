using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EndGamePopUpController : MonoBehaviour 
{
	public void reset(bool hasWon)
	{
		string text = "";
		if(hasWon)
		{
			text="BRAVO !\n\nVenez en match officiel vous mesurer aux meilleurs joueurs !";
		}
		else
		{
			text="DOMMAGE !\n\nC'est en s'entrainant qu'on progresse ! Courage !";
		}
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = text;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Continuer";
		gameObject.transform.FindChild ("Button").GetComponent<EndGamePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<EndGamePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		NewHomePageController.instance.hideEndGamePopUp ();
	}
}

