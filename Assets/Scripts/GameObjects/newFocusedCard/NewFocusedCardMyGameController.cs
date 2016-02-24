using UnityEngine;
using TMPro;

public class NewFocusedCardMyGameController : NewFocusedCardController
{
	public override void initializeFocusFeatures()
	{
	}

	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(true);
		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(1);
		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="+ " + ApplicationDesignRules.priceToString(this.c.destructionPrice);

		this.gameObject.transform.FindChild("FocusFeature1").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(3);
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature1").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="- " + ApplicationDesignRules.priceToString(this.c.NextLevelPrice);
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().showPrice(true);
			if(this.c.NextLevelPrice>ApplicationModel.player.Money)
			{
				this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
			}
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().showPrice(false);
			this.gameObject.transform.FindChild("FocusFeature1").transform.GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		this.gameObject.transform.FindChild("FocusFeature2").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(2);
		this.gameObject.transform.FindChild("FocusFeature2").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="- " + ApplicationDesignRules.priceToString(this.c.RenameCost);
		this.gameObject.transform.FindChild("FocusFeature2").GetComponent<NewFocusedFeaturesController>().showPrice(true);

		if(this.c.RenameCost>ApplicationModel.player.Money)
		{
			this.gameObject.transform.FindChild("FocusFeature2").GetComponent<NewFocusedFeaturesController>().setIsClickable(false);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature2").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
		}
		
		this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+WordingFocusedCard.getReference(9) + this.c.nbLoose+WordingFocusedCard.getReference(10);
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setIsClickable(true);
	}
	public override void refreshCredits()
	{
		StartCoroutine(BackOfficeController.instance.getUserData ());
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
		case 4:
			this.exitCard();
			break;
		}
	}
}

