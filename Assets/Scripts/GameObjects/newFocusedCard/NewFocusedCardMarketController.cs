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
	}
	public override void updateFocusFeatures()
	{
		if(this.c.Price>ApplicationModel.credits && this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Il vous manque "+(this.c.Price-ApplicationModel.credits)+" cristaux pour acheter cette carte.";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
			this.gameObject.transform.FindChild("FocusFeature1").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("FocusFeature2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("FocusFeature3").gameObject.SetActive(false);
		}
		else if(this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			this.gameObject.transform.FindChild("FocusFeature0").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Unité en vente pour "+this.c.Price+" cristaux. \nAcheter ?";
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
			this.gameObject.transform.FindChild("FocusFeature1").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("FocusFeature2").gameObject.SetActive(false);
			this.gameObject.transform.FindChild("FocusFeature3").gameObject.SetActive(false);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("FocusFeature2").gameObject.SetActive(true);
			this.gameObject.transform.FindChild("FocusFeature3").gameObject.SetActive(true);
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
			if(this.c.onSale==1)
			{
				this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Unité en vente sur le marché pour "+this.c.Price+" cristaux. \nModifier ?";
				this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature3").transform.FindChild("Title").GetComponent<TextMeshPro>().text="Mettre l'unité en vente sur le marché";
				this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
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
		if(this.c.onSale==1 && this.c.IdOWner!=NewMarketController.instance.returnUserId())
		{
			switch(feature)
			{
			case 0:
				this.displayBuyCardPopUp();
				break;
			case 5:
				this.exitCard();
				break;
			}
		}
		else
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
				this.exitCard();
				break;
			}
		}
	}
	public override void actualizePrice()
	{
		this.updateFocusFeatures ();
	}
}

