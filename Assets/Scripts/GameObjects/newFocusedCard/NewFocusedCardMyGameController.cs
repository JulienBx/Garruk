using UnityEngine;
using TMPro;

public class NewFocusedCardMyGameController : NewFocusedCardController
{
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
		if(this.c.Decks.Count>0)
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Carte attachée à un deck, ne pouvant être mise en vente";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
			
		}
		else if(this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Carte en vente sur le bazar pour "+this.c.Price+" crédits. \nModifier ?";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Mettre la carte en vente sur le bazar";
			this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		this.gameObject.transform.FindChild("FocusFeature4").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+" Victoires \n" + this.c.nbLoose+" Défaites";
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
	}
	public override void refreshCredits()
	{
		newMyGameController.instance.refreshCredits ();
	}
	public override void deleteCard ()
	{
		base.exitFocus ();
		newMyGameController.instance.deleteCard ();
	}
	public override void exitFocus()
	{
		base.exitFocus ();
		newMyGameController.instance.hideCardFocused ();
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

