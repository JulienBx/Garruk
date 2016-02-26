using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class NewDeckPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNewDeckPopUp.getReference(0);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingNewDeckPopUp.getReference(1);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Button").GetComponent<NewDeckPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<NewDeckPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void newDeckHandler()
	{
		newMyGameController.instance.createNewDeckHandler();
	}
	public void exitPopUp()
	{
		newMyGameController.instance.hideNewDeckPopUp ();
	}
	public string getInputText()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

