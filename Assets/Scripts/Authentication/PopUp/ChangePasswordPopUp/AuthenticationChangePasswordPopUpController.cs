using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class AuthenticationChangePasswordPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Information1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<AuthenticationChangePasswordPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<AuthenticationChangePasswordPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingAuthenticationChangePasswordPopUp.getReference(0);
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingAuthenticationChangePasswordPopUp.getReference(1);
		gameObject.transform.FindChild("Information1").GetComponent<TextMeshPro> ().text = WordingAuthenticationChangePasswordPopUp.getReference(2);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingAuthenticationChangePasswordPopUp.getReference(3);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingAuthenticationChangePasswordPopUp.getReference(4);
	}
	public void resize()
	{
		gameObject.transform.FindChild("Input1").GetComponent<InputPasswordGuiController> ().resize ();
		gameObject.transform.FindChild("Input2").GetComponent<InputPasswordGuiController>().resize();
		this.computeLabels();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void changePasswordHandler()
	{
		AuthenticationController.instance.inscriptionHandler();
	}
	public string getFirstPassword()
	{
		return gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getSecondPassword()
	{
		return gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideChangePasswordPopUp();
	}
}

