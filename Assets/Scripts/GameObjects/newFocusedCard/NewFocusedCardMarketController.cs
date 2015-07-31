using UnityEngine;
using TMPro;

public class NewFocusedCardMarketController : NewFocusedCardController
{

	public override void show()
	{
		base.show ();
		if(this.c.onSale==0)
		{
			this.displayPanelSold();
		}
	}
	public override void initializeFocusFeatures()
	{
		this.gameObject.transform.FindChild ("FocusFeature1").gameObject.SetActive (false);
		this.gameObject.transform.FindChild ("FocusFeature2").gameObject.SetActive (false);
		this.gameObject.transform.FindChild ("FocusFeature3").gameObject.SetActive (false);
	}
	public override void updateFocusFeatures()
	{

		if(this.c.Price>ApplicationModel.credits && this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Il vous manque "+(this.c.Price-ApplicationModel.credits)+" crédits pour acheter cette carte.";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		else if(this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Carte en vente pour "+this.c.Price+" crédits. \nAcheter ?";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Cette carte n'est plus en vente";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature4").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+" Victoires \n" + this.c.nbLoose+" Défaites";
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Quitter";
	}
	public override void refreshCredits()
	{
		NewMarketController.instance.refreshCredits ();
	}
	public override void exitFocus()
	{
		base.exitFocus ();
		NewMarketController.instance.hideCardFocused ();
	}
	public override void focusFeaturesHandler(int id)
	{
		switch(id)
		{
		case 0:
			this.displayBuyCardPopUp();
			break;
		case 5:
			this.exitFocus();
			break;
		}
	}
	public override void actualizePrice()
	{
		this.updateFocusFeatures ();
	}
}
