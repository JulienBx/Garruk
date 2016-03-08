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
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title3").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("InscriptionButton").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("LostLoginButton").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText (login);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<LoginPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("InscriptionButton").GetComponent<LoginPopUpInscriptionButtonController>().reset();
		gameObject.transform.FindChild("LostLoginButton").GetComponent<LoginPopUpLostLoginButtonController>().reset();
		this.applyRememberMeSprites();
	}
	public void computeLabels()
	{
		if(ApplicationDesignRules.isMobileDevice) // A remplacer
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(7);
		}
		else
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(6);
		}
		gameObject.transform.FindChild ("Title1").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(1);
		gameObject.transform.FindChild ("Title3").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(2);
		gameObject.transform.FindChild("InscriptionButton").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(4);
		gameObject.transform.FindChild("LostLoginButton").GetComponent<TextMeshPro>().text=WordingLoginPopUp.getReference(5);
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(3);
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
	public void rememberMeHandler()
	{
		SoundController.instance.playSound(8);
		this.rememberMe=!this.rememberMe;
		this.applyRememberMeSprites();
	}
	public void loginHandler()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.loginHandler();
	}
	public void inscriptionHandler()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.displayInscriptionPopUp();
		AuthenticationController.instance.hideLoginPopUp();
	}
	public void lostLoginHandler()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.displayLostLoginPopUp();
		AuthenticationController.instance.hideLoginPopUp();
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
		return gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().getText ();
	}
	public bool getRememberMe()
	{
		return this.rememberMe;
	}
}

