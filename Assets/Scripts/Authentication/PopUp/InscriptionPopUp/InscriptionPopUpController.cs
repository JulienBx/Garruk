using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class InscriptionPopUpController : MonoBehaviour 
{
	public void reset()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Information1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Information2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		gameObject.transform.FindChild("Title3").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title4").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input1").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input3").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input4").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<InscriptionPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<InscriptionPopUpCloseButtonController>().reset();
	}
	public void computeLabels()
	{
		if(ApplicationDesignRules.isMobileScreen) // A remplacer par MobileDevice
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingInscriptionPopUp.getReference(8);
		}
		else
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingInscriptionPopUp.getReference(0);
		}
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(1);
		gameObject.transform.FindChild("Information1").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(2);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(3);
		gameObject.transform.FindChild("Information2").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(4);
		gameObject.transform.FindChild ("Title3").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(5);
		gameObject.transform.FindChild ("Title4").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(6);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(7);
	}
	public void resize()
	{
		gameObject.transform.FindChild("Input1").GetComponent<InputTextGuiController> ().resize ();
		gameObject.transform.FindChild("Input2").GetComponent<InputPasswordGuiController>().resize();
		gameObject.transform.FindChild("Input3").GetComponent<InputPasswordGuiController>().resize();
		gameObject.transform.FindChild("Input4").GetComponent<InputTextGuiController>().resize();
		this.computeLabels();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void inscriptionHandler()
	{
		AuthenticationController.instance.inscriptionHandler();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input1").GetComponent<InputTextGuiController> ().getText ();
	}
	public string getFirstPassword()
	{
		return gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getSecondPassword()
	{
		return gameObject.transform.FindChild ("Input3").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getEmail()
	{
		return gameObject.transform.FindChild ("Input4").GetComponent<InputTextGuiController> ().getText ();
	}
	public void exitPopUp()
	{
		AuthenticationController.instance.displayLoginPopUp();
	}
}

