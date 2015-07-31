using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class CupGameButtonController : LobbyButtonController
{

	private Cup cup;

	public void setCup(Cup cup)
	{
		this.cup = cup;
	}
	public override void drawBackSide()
	{
		gameObject.transform.FindChild("Button").FindChild("CupName").GetComponent<TextMeshPro>().text=this.cup.Name;
		int nbGamesCup = this.getNbGamesCup ();
		if(nbGamesCup>0)
		{
			gameObject.transform.FindChild("Button").FindChild("RemainingRounds").GetComponent<TextMeshPro>().text="Maths restants : "+(this.cup.NbRounds-nbGamesCup);	
		}
		else
		{
			gameObject.transform.FindChild("Button").FindChild("RemainingRounds").GetComponent<TextMeshPro>().text="Compétition non démarrée";
		}
		gameObject.transform.FindChild("Button").FindChild("Prize").GetComponent<TextMeshPro>().text="Récompense : "+this.cup.CupPrize+" crédits";
	}
	public override void drawFrontSide()
	{
		gameObject.transform.FindChild("Button").FindChild ("CupName").GetComponent<TextMeshPro>().text=null;
		gameObject.transform.FindChild("Button").FindChild ("RemainingRounds").GetComponent<TextMeshPro> ().text = null;
		gameObject.transform.FindChild("Button").FindChild ("Prize").GetComponent<TextMeshPro> ().text = null;
	}
	public int getNbGamesCup()
	{
		return NewLobbyController.instance.getNbGamesCup ();
	}
}

