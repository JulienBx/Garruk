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
			text=WordingEndGamePopUp.getReference(0);
		}
		else
		{
			text=WordingEndGamePopUp.getReference(1);
		}
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = text;
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEndGamePopUp.getReference(2);
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

