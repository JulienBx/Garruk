using UnityEngine;
using TMPro;

public class NewFocusedCardStoreController : NewFocusedCardController
{
	public void displayFocusFeatures(bool value)
	{
		this.gameObject.transform.FindChild ("FocusFeature0").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature1").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature2").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature3").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature4").gameObject.SetActive (value);
		this.gameObject.transform.FindChild ("FocusFeature5").gameObject.SetActive (value);
	}
	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Title").GetComponent<TextMeshPro>().text="Désintégrer \n( +"+this.c.destructionPrice+" crédits)";
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Passer la carte au niveau suivant \n( -"+this.c.NextLevelPrice+ " crédits)";
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Title").GetComponent<TextMeshPro>().text="Niveau maximum atteint";
			this.gameObject.transform.FindChild("FocusFeature1").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature2").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Renommer la carte \n( -" + this.c.RenameCost + " crédits)";

		if(this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Carte en vente sur le bazar pour "+this.c.Price+" crédits. \nModifier ?";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Mettre la carte en vente sur le bazar";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
	}
	public override void refreshCredits()
	{
		NewStoreController.instance.refreshCredits ();
	}
	public override void deleteCard ()
	{
		base.exitFocus ();
		NewStoreController.instance.deleteCard ();
	}
	public override void exitFocus()
	{
		base.exitFocus ();
		NewStoreController.instance.hideCardFocused ();
	}
	public override void focusFeaturesHandler(int id)
	{
		switch(id)
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
		case 3:
			if(c.onSale==1)
			{
				base.displayEditSellCardPopUp();
			}
			else
			{
				base.displayputOnMarketCardPopUp();
			}
			break;
		case 5:
			this.exitFocus();
			break;
		}
	}
}

