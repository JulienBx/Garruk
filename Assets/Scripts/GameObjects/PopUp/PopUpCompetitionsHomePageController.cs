using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpCompetitionsHomePageController : PopUpController
{
	
	public override void startHoveringPopUp()
	{
		NewHomePageController.instance.startHoveringPopUp();
	}
	public override void endHoveringPopUp()
	{
		NewHomePageController.instance.endHoveringPopUp();
	}
	public void showCup(Cup c)
	{
		gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = c.Name;
		int nbGamesCup = NewHomePageController.instance.getNbGamesCup ();
		if(nbGamesCup>0)
		{
			gameObject.transform.FindChild ("remainingGames").GetComponent<TextMeshPro> ().text ="Matchs restants : "+(c.NbRounds-nbGamesCup);
		}
		else
		{
			gameObject.transform.FindChild ("remainingGames").GetComponent<TextMeshPro> ().text = "Coupe non démarrée";
		}
		gameObject.transform.FindChild ("rewardTitle").GetComponent<TextMeshPro> ().text = "Récompense";
		gameObject.transform.FindChild ("rewardValue").GetComponent<TextMeshPro> ().text = c.CupPrize.ToString ();
		gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().sprite = c.texture;
	}
	public void showDivision(Division d)
	{
		gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = d.Name;
		int nbGamesDivision = NewHomePageController.instance.getNbGamesDivision ();
		if(nbGamesDivision>0)
		{
			gameObject.transform.FindChild ("remainingGames").GetComponent<TextMeshPro> ().text ="Matchs restants : "+(d.NbGames-nbGamesDivision);
		}
		else
		{
			gameObject.transform.FindChild ("remainingGames").GetComponent<TextMeshPro> ().text = "League non démarrée";
		}
		gameObject.transform.FindChild ("rewardTitle").GetComponent<TextMeshPro> ().text = "Récompense";
		gameObject.transform.FindChild ("rewardValue").GetComponent<TextMeshPro> ().text = d.TitlePrize.ToString ();
		gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().sprite = d.texture;
	}
}

