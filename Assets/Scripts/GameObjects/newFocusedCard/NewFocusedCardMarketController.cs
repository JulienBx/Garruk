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
	}
	public override void updateFocusFeatures()
	{
		this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().reset();
		this.gameObject.transform.FindChild("FocusFeature3").GetComponent<NewFocusedFeaturesFeature3Controller>().reset();
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().reset();

		if(this.c.IdOWner!=NewMarketController.instance.returnUserId() && this.c.onSale==1)
		{
			this.gameObject.transform.FindChild("FocusFeature0").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(5);
			this.gameObject.transform.FindChild("FocusFeature0").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text="- " + ApplicationDesignRules.priceToString(this.c.Price);
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(true);
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(27),WordingFocusedCard.getReference(28));
			this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setId(0);
			if(this.c.Price>ApplicationModel.player.Money  && this.c.IdOWner!=NewMarketController.instance.returnUserId())
			{
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(false);
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(true);
			}
		}
		else
		{
			if(this.c.onSale==1)
			{
				this.gameObject.transform.FindChild("FocusFeature0").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(0);
				this.gameObject.transform.FindChild("FocusFeature0").FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().text=ApplicationDesignRules.priceToString(this.c.Price);
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(true);
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(true);
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(29),WordingFocusedCard.getReference(30));
			}
			else if(this.c.IdOWner==NewMarketController.instance.returnUserId())
			{
				this.gameObject.transform.FindChild("FocusFeature0").FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().sprite=this.returnFocusFeaturePicto(4);
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().showPrice(false);
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(true);
				this.gameObject.transform.FindChild("FocusFeature0").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(25),WordingFocusedCard.getReference(26));
			}
			else
			{
				this.gameObject.transform.FindChild("FocusFeature0").transform.GetComponent<NewFocusedFeaturesController>().setIsActive(false);
			}
		}
		this.gameObject.transform.FindChild("FocusFeature3").transform.GetComponent<TextMeshPro>().text=this.c.nbWin+WordingFocusedCard.getReference(9) + this.c.nbLoose+WordingFocusedCard.getReference(10);
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setIsActive(true);
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setToolTip(WordingFocusedCard.getReference(31),WordingFocusedCard.getReference(32));
		this.gameObject.transform.FindChild("FocusFeature4").GetComponent<NewFocusedFeaturesController>().setId(4);
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
		base.selectAFeature(feature);
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
		case 4:
			this.exitCard();
			break;
		}
	}
	public override void actualizePrice()
	{
		this.updateFocusFeatures ();
	}
}

