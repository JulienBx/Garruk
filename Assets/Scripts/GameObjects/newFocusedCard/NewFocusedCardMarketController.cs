using UnityEngine;
using TMPro;

public class NewFocusedCardMarketController : NewFocusedCardController
{

	public override void show()
	{
		base.show ();
		if(this.c.onSale==0 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			this.displayPanelSold();
		}
	}
	public override void initializeFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature1").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("FocusFeature2").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("FocusFeature3").gameObject.SetActive(false);
	}
	public override void updateFocusFeatures()
	{
		if(this.c.Price>ApplicationModel.credits && this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Il vous manque "+(this.c.Price-ApplicationModel.credits)+" cristaux pour acheter cette carte.";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);

		}
		else if(this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Unité en vente pour "+this.c.Price+" cristaux. \nAcheter ?";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		else
		{
			if(this.c.onSale==1)
			{
				this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Unité en vente sur le marché pour "+this.c.Price+" cristaux. \nModifier ?";
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Mettre l'unité en vente sur le marché";
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
			}
		}
		this.gameObject.transform.FindChild("FocusFeature4").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+" Victoires \n" + this.c.nbLoose+" Défaites";
		this.gameObject.transform.FindChild("FocusFeature5").transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Retour";
		this.gameObject.transform.FindChild("FocusFeature5").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
	}
	public override void refreshCredits()
	{
		NewMarketController.instance.refreshCredits ();
	}
	public override void deleteCard ()
	{
		base.exitCard ();
		NewMarketController.instance.deleteCard ();
	}
	public override void goBackToScene()
	{
		NewMarketController.instance.updateScene ();
		NewMarketController.instance.hideCardFocused ();
	}
	public override void selectAFeature(int feature)
	{
		switch(feature)
		{
		case 0:
			if(this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
			{
				this.displayBuyCardPopUp();
			}
			else if(c.onSale==1)
			{
				base.displayEditSellCardPopUp();
			}
			else
			{
				base.displayputOnMarketCardPopUp();
			}
			break;
		case 5:
			this.exitCard();
			break;
		}
	}
	public override void actualizePrice()
	{
		this.updateFocusFeatures ();
	}
}

