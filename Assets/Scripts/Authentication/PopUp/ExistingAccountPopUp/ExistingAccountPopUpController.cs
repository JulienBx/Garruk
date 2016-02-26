using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class ExistingAccountPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("LostLoginButton").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<ExistingAccountPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("LostLoginButton").GetComponent<ExistingAccountPopUpLostLoginButtonController>().reset();
		gameObject.transform.FindChild("CloseButton").GetComponent<ExistingAccountPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingExistingAccountPopUp.getReference(0);
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingExistingAccountPopUp.getReference(1);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingExistingAccountPopUp.getReference(2);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingExistingAccountPopUp.getReference(3);
		gameObject.transform.FindChild("LostLoginButton").GetComponent<TextMeshPro>().text=WordingExistingAccountPopUp.getReference(4);
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
		gameObject.transform.FindChild("Input1").GetComponent<InputPasswordGuiController>().resize();
		this.computeLabels();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void existingAccountHandler()
	{
		AuthenticationController.instance.existingAccountHandler();
	}
	public void lostLoginHandler()
	{
		AuthenticationController.instance.displayLostLoginPopUp();
		AuthenticationController.instance.hideExistingAccountPopUp();
	}
	public string getPassword()
	{
		return gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideExistingAccountPopUp();
	}
}

