using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EditDeckPopUpController : MonoBehaviour 
{
	public void reset(string deckName)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditDeckPopUp.getReference(0);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingEditDeckPopUp.getReference(1);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (deckName);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Button").GetComponent<EditDeckPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<EditDeckPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void editDeckHandler()
	{
		newMyGameController.instance.editDeckHandler();
	}
	public void exitPopUp()
	{
		newMyGameController.instance.hideEditDeckPopUp ();
	}
	public string getInputText()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

