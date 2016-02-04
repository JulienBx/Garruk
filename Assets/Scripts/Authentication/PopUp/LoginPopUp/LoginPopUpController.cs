using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class LoginPopUpController : MonoBehaviour 
{
	public Sprite[] rememberMeButtonSprites;
	private bool rememberMe;

	public void reset(string login, bool rememberMe)
	{
		this.rememberMe=rememberMe;
		this.computeLabels();
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title3").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("InscriptionButton").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("ForgotPasswordButton").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (login);
		gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<LoginPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("InscriptionButton").GetComponent<LoginPopUpInscriptionButtonController>().reset();
		this.applyRememberMeSprites();
	}
	public void computeLabels()
	{
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(1);
		gameObject.transform.FindChild ("Title3").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(2);
		gameObject.transform.FindChild("InscriptionButton").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(4);
		gameObject.transform.FindChild("ForgotPasswordButton").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(5);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(3);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(6);
	}
	public void resize()
	{
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().resize ();
		gameObject.transform.FindChild("Input1").GetComponent<InputPasswordGuiController>().resize();
	}
	public void setError(string error)
	{
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = error;
	}
	public void rememberMeHandler()
	{
		this.rememberMe=!this.rememberMe;
		this.applyRememberMeSprites();
	}
	public void loginHandler()
	{
		AuthenticationController.instance.loginHandler();
	}
	public void inscriptionHandler()
	{
		AuthenticationController.instance.displayInscriptionPopUp();
	}
	public void applyRememberMeSprites()
	{
		if(this.rememberMe)
		{
			gameObject.transform.FindChild("RememberMeButton").GetComponent<SpriteRenderer>().sprite=this.rememberMeButtonSprites[0];
		}
		else
		{
			gameObject.transform.FindChild("RememberMeButton").GetComponent<SpriteRenderer>().sprite=this.rememberMeButtonSprites[1];
		}
	}
	public string getPassword()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public bool getRememberMe()
	{
		return this.rememberMe;
	}
}

