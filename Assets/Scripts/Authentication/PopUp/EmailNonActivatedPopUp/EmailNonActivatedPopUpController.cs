using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class EmailNonActivatedPopUpController : MonoBehaviour 
{
	public void reset(string mail)
	{
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (mail);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Button").GetComponent<EmailNonActivatedPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild ("Button2").GetComponent<EmailNonActivatedPopUpCancelButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<EmailNonActivatedPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEmailNonActivatedPopUp.getReference(0);
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingEmailNonActivatedPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEmailNonActivatedPopUp.getReference(2);
		gameObject.transform.FindChild ("Button2").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingEmailNonActivatedPopUp.getReference(3);
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void emailNonActivatedHandler()
	{
		AuthenticationController.instance.emailNonActivatedHandler();
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideEmailNonActivatedPopUp();
	}
	public string getEmail()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

