using UnityEngine;
using TMPro;

public class NewFocusedCardHomePageController : NewFocusedCardController
{
	public override void initializeFocusFeatures()
	{
		this.gameObject.transform.FindChild ("FocusFeature2").gameObject.SetActive (false);
	}
	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().reset();
		this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().reset();
		this.gameObject.transform.FindChild("FocusFeature3").GetComponent<NewFocusedFeaturesFeature3Controller>().reset();
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().reset();

		this.gameObject.transform.FindChild("FocusFeature0").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(3);
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(33),WordingFocusedCard.getReference(34));
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setId(0);
		if(this.c.ExperienceLevel!=10)
		{
			this.gameObject.transform.FindChild("FocusFeature0").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="- " + ApplicationDesignRules.priceToString(this.c.NextLevelPrice);
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(true);
			if(this.c.NextLevelPrice>ApplicationModel.player.Money)
			{
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setIsActive(false);
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setIsActive(true);
			}
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(false);
			this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(false);
		}
		this.gameObject.transform.FindChild("FocusFeature1").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(2);
		this.gameObject.transform.FindChild("FocusFeature1").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="- " + ApplicationDesignRules.priceToString(this.c.RenameCost);
		this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().showPrice(true);
		this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(21),WordingFocusedCard.getReference(22));
		this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setId(1);

		if(this.c.RenameCost>ApplicationModel.player.Money)
		{
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsActive(false);
		}
		else
		{
			this.gameObject.transform.FindChild("FocusFeature1").GetComponent<NewFocusedFeaturesController>().setIsActive(true);
		}

		this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+WordingFocusedCard.getReference(9) + this.c.nbLoose+WordingFocusedCard.getReference(10);
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setIsActive(true);
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(31),WordingFocusedCard.getReference(32));
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setId(4);
	}
	public override void refreshCredits()
	{
		NewHomePageController.instance.refreshCredits ();
	}
	public override void goBackToScene()
	{
		NewHomePageController.instance.hideCardFocused ();
	}
	public override void selectAFeature(int feature)
	{
		base.selectAFeature(feature);
		switch(feature)
		{
		case 0:
			base.displayBuyXpCardPopUp();
			break;
		case 1:
			base.displayRenameCardPopUp();
			break;
		case 4:
			this.exitCard();
			break;
		}
	}
}

