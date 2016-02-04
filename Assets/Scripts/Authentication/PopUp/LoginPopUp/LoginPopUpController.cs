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
		gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(0);
		gameObject.transform.FindChild ("Title2").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(1);
		gameObject.transform.FindChild ("Title3").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(2);
		gameObject.transform.FindChild ("Title4").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(3);
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Button").FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingLoginPopUp.getReference(4);
		gameObject.transform.FindChild ("Input").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input1").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<LoginPopUpConfirmButtonController> ().reset ();
		this.applyRememberMeSprites();
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
	}
	public void loginHandler()
	{
		this.rememberMe=!this.rememberMe;
		this.applyRememberMeSprites();
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
		return gameObject.transform.FindChild ("Input").GetComponent<InputPasswordGuiController> ().getText ();
	}
	public string getLogin()
	{
		return gameObject.transform.FindChild ("Input1").GetComponent<InputTextGuiController> ().getText ();
	}
	public bool getRememberMe()
	{
		return this.rememberMe;
	}
}

