using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class LostLoginPopUpController : MonoBehaviour 
{
	public void reset(string login)
	{
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (login);
		gameObject.transform.FindChild ("Button").GetComponent<LostLoginPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<LostLoginPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLostLoginPopUp.getReference(0);
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingLostLoginPopUp.getReference(1);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLostLoginPopUp.getReference(2);
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void lostLoginHandler()
	{
		AuthenticationController.instance.lostLoginHandler();
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideLostLoginPopUp();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
}

