using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class DivisionGameButtonController : LobbyButtonController
{

	private Division division;
	
	public void setDivision(Division division)
	{
		this.division = division;
	}
	public override void drawBackSide()
	{
		gameObject.transform.FindChild("Button").FindChild("DivisionName").GetComponent<TextMeshPro>().text=this.division.Name;
		int nbGamesDivision = this.getNbGamesDivision ();
		if(nbGamesDivision>0)
		{
			gameObject.transform.FindChild("Button").FindChild("RemainingGames").GetComponent<TextMeshPro>().text="Maths restants : "+(this.division.NbGames-nbGamesDivision);	
		}
		else
		{
			gameObject.transform.FindChild("Button").FindChild("RemainingGames").GetComponent<TextMeshPro>().text="Compétition non démarrée";
		}
		gameObject.transform.FindChild("Button").FindChild("Prize").GetComponent<TextMeshPro>().text="Récompense : "+this.division.TitlePrize+" crédits";
	}
	public override void drawFrontSide()
	{
		gameObject.transform.FindChild("Button").FindChild ("DivisionName").GetComponent<TextMeshPro>().text=null;
		gameObject.transform.FindChild("Button").FindChild ("RemainingGames").GetComponent<TextMeshPro> ().text = null;
		gameObject.transform.FindChild("Button").FindChild ("Prize").GetComponent<TextMeshPro> ().text = null;
	}
	public int getNbGamesDivision()
	{
		return NewLobbyController.instance.getNbGamesDivision ();
	}
}

