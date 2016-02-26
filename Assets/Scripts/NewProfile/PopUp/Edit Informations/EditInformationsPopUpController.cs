using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EditInformationsPopUpController : MonoBehaviour 
{
	public void reset(string input1, string input2, string input3)
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEditInformationsPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text =WordingEditInformationsPopUp.getReference(1);
		gameObject.transform.FindChild ("Title3").GetComponent<TextMeshPro> ().text = WordingEditInformationsPopUp.getReference(2);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text =WordingEditInformationsPopUp.getReference(3);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (input1);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Input2").GetComponent<InputTextGuiController> ().setText (input2);
		gameObject.transform.FindChild ("Input3").GetComponent<InputTextGuiController> ().setText (input3);
		gameObject.transform.FindChild ("Button").GetComponent<EditInformationsPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("CloseButton").GetComponent<EditInformationsPopUpCloseButtonController> ().reset ();
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
		gameObject.transform.FindChild ("Input2").GetComponent<InputTextGuiController> ().resize ();
		gameObject.transform.FindChild ("Input3").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void editInformationsHandler()
	{
		NewProfileController.instance.updateUserInformationsHandler();
	}
	public void exitPopUp()
	{
		NewProfileController.instance.hideEditInformationsPopUp ();
	}
	public string getFirstInput()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
	public string getSecondInput()
	{
		return gameObject.transform.FindChild ("Input2").GetComponent<InputTextGuiController> ().getText ();
	}
	public string getThirdInput()
	{
		return gameObject.transform.FindChild ("Input3").GetComponent<InputTextGuiController> ().getText ();
	}
}

