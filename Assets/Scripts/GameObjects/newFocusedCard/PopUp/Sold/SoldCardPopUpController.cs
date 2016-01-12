using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class SoldCardPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Votre carte vient d'être vendue.";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
		gameObject.transform.FindChild ("Button").GetComponent<SoldPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<SoldPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideSoldCardPopUp ();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

