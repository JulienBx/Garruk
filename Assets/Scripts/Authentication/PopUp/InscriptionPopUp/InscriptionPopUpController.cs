﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class InscriptionPopUpController : MonoBehaviour 
{

	public Sprite[] touButtonSprites;
	private bool touAcepted;

	public void reset()
	{
		this.touAcepted=false;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Information1").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		gameObject.transform.FindChild("Title2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Information2").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		gameObject.transform.FindChild("Title3").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Title4").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild ("Error").GetComponent<TextMeshPro> ().text = "";
		gameObject.transform.FindChild ("Input1").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input1").GetComponent<InputTextGuiController> ().setFocused();
		gameObject.transform.FindChild ("Input2").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input3").GetComponent<InputPasswordGuiController> ().setText ("");
		gameObject.transform.FindChild ("Input4").GetComponent<InputTextGuiController> ().setText ("");
		gameObject.transform.FindChild ("Button").GetComponent<InscriptionPopUpConfirmButtonController> ().reset ();
		gameObject.transform.FindChild("CloseButton").GetComponent<InscriptionPopUpCloseButtonController>().reset();
		this.applyTouSprites();
	}
	public void computeLabels()
	{
		if(ApplicationDesignRules.isMobileDevice) // A remplacer
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
		gameObject.transform.FindChild ("Information3").GetComponent<TextMeshPro> ().text = WordingInscriptionPopUp.getReference(9);
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
	public void touHandler()
	{
		SoundController.instance.playSound(8);
		this.touAcepted=!this.touAcepted;
		this.applyTouSprites();
	}
	public void inscriptionHandler()
	{
		SoundController.instance.playSound(8);
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
	public bool getTouAccepted()
	{
		return this.touAcepted;
	}
	public void exitPopUp()
	{
		SoundController.instance.playSound(8);
		AuthenticationController.instance.displayLoginPopUp();
		AuthenticationController.instance.hideInscriptionPopUp();
	}
	public void applyTouSprites()
	{
		if(this.touAcepted)
		{
			gameObject.transform.FindChild("TouButton").GetComponent<SpriteRenderer>().sprite=this.touButtonSprites[0];
		}
		else
		{
			gameObject.transform.FindChild("TouButton").GetComponent<SpriteRenderer>().sprite=this.touButtonSprites[1];
		}
	}
	public void touLinkHandler()
    {
		if(TMP_TextUtilities.FindIntersectingLink(gameObject.transform.FindChild ("Information3").GetComponent<TextMeshPro> (),Input.mousePosition,Camera.main)!=-1)
        {
			Application.OpenURL(WordingInscriptionPopUp.getReference(10));
        }
    }
}

