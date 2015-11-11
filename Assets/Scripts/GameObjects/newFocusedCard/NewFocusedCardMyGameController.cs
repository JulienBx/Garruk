using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;

public class NewFocusedCardMyGameController : NewFocusedCardController
{
	public override void initializeFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature3").gameObject.SetActive(false);
	}
	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Title").GetComponent<TextMeshPro>().text="Bannir \n( +"+this.c.destructionPrice+" cristaux)";
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Passer l'unité au niveau suivant \n( -"+this.c.NextLevelPrice+ " cristaux)";
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Niveau maximum atteint";
			this.gameObject.transform.FindChild("FocusFeature1").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature2").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Renommer l'unité \n( -" + this.c.RenameCost + " cristaux)";
		this.gameObject.transform.FindChild("FocusFeature2").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		this.gameObject.transform.FindChild("FocusFeature4").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+" Victoires \n" + this.c.nbLoose+" Défaites";
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Retour";
		this.gameObject.transform.FindChild("FocusFeature5").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
	}
	public override void refreshCredits()
	{
		StartCoroutine(MenuController.instance.getUserData ());
	}
	public override void buyXpCardHandler()
	{
		base.buyXpCardHandler ();
	}
	public override void deleteCard ()
	{
		base.exitCard ();
		newMyGameController.instance.deleteCard ();
	}
	public override void goBackToScene()
	{
		newMyGameController.instance.hideCardFocused ();
	}
	public override void selectAFeature(int feature)
	{
		switch(feature)
		{
		case 0:
			base.displaySellCardPopUp();
			break;
		case 1:
			base.displayBuyXpCardPopUp();
			break;
		case 2:
			base.displayRenameCardPopUp();
			break;
		case 5:
			this.exitCard();
			break;
		}
	}
}

