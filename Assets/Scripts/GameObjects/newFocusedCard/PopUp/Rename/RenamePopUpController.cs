using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class RenamePopUpController : MonoBehaviour 
{
	public void reset(int price, string name)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingRenamePopUp.getReference(0) + price + WordingRenamePopUp.getReference(1);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingRenamePopUp.getReference(2);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (name);
		gameObject.transform.FindChild ("Button").GetComponent<RenamePopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<RenamePopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void renameHandler()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().renameCardHandler ();
	}
	public void exitPopUp()
	{
		gameObject.transform.parent.GetComponent<NewFocusedCardController> ().hideRenamePopUp ();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

