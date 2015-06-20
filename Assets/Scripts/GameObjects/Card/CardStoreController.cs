using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class CardStoreController : CardController
{

	private FocusStoreFeaturesView focusStoreFeaturesView;
	private bool isTutorialLaunched;

	public CardStoreController  ()
	{
	}
	void OnMouseOver()
	{
		if (Input.GetMouseButton(1)) 
		{
			StoreController.instance.rightClickedCard (gameObject);
		}
	}
	public void setStoreCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
	}
	public void resetStoreCard(Card c)
	{
		this.eraseCard ();
		this.setStoreCard (c);
	}
	public void resetFocusedStoreCard(Card c)
	{
		this.eraseCard ();
		this.setStoreCard (c);
		this.setFocusStoreFeatures ();
	}
	public override void eraseCard()
	{
		base.eraseCard ();
		if(focusStoreFeaturesView!=null)
		{
			Destroy (this.focusStoreFeaturesView);
		}
	}
	public override void updateExperience()
	{
		base.updateExperience ();
		focusStoreFeaturesView.focusStoreFeaturesVM.cardLevel = base.card.ExperienceLevel;
		focusStoreFeaturesView.focusStoreFeaturesVM.nextLevelCost = base.card.NextLevelPrice;
		focusStoreFeaturesView.focusStoreFeaturesVM.cardCost = base.card.destructionPrice;
	}
	public void setFocusStoreFeatures()
	{
		this.focusStoreFeaturesView = gameObject.AddComponent<FocusStoreFeaturesView> ();
		focusStoreFeaturesView.focusStoreFeaturesVM.renameCost = base.card.RenameCost;
		focusStoreFeaturesView.focusStoreFeaturesVM.isOnSale = System.Convert.ToBoolean (base.card.onSale);
		focusStoreFeaturesView.focusStoreFeaturesVM.price = base.card.Price;
		focusStoreFeaturesView.focusStoreFeaturesVM.cardCost = base.card.destructionPrice;
		focusStoreFeaturesView.focusStoreFeaturesVM.cardLevel = base.card.ExperienceLevel;
		focusStoreFeaturesView.focusStoreFeaturesVM.nextLevelCost = base.card.NextLevelPrice;
		focusStoreFeaturesView.cardFeaturesFocusVM.styles=new GUIStyle[ressources.cardFeaturesFocusStyles.Length];
		for(int i=0;i<ressources.cardFeaturesFocusStyles.Length;i++)
		{
			focusStoreFeaturesView.cardFeaturesFocusVM.styles[i]=ressources.cardFeaturesFocusStyles[i];
		}
		focusStoreFeaturesView.cardFeaturesFocusVM.initStyles();
		this.focusStoreFeaturesResize ();
	}
	public void focusStoreFeaturesResize()
	{
		for(int i=0;i<4;i++)
		{
			focusStoreFeaturesView.focusStoreFeaturesVM.cardFeaturesFocusRects[i]=base.getCardFeaturesFocusRect(i);
		}
		focusStoreFeaturesView.focusStoreFeaturesVM.cardFeaturesFocusRects[4]=base.getCardFeaturesFocusRect(5);
		focusStoreFeaturesView.cardFeaturesFocusVM.resize(base.getCardFeaturesFocusRect(0).height);
	}
	public override void resize()
	{
		base.resize ();
		if(this.focusStoreFeaturesView!=null)
		{                                                            
			this.focusStoreFeaturesResize();
		}
	}
	public void exitFocus()
	{
		StoreController.instance.exitCard (); 
		if(isTutorialLaunched)
		{
			StoreController.instance.tutorialCardLeaved();
		}
	}
	public override void setGUI(bool value)
	{
		StoreController.instance.setGUI (value);
	}
	public override void setMyGUI(bool value)
	{
		if(this.focusStoreFeaturesView!=null)
		{
			focusStoreFeaturesView.cardFeaturesFocusVM.guiEnabled=value;
			this.setExitButtonsGui (value);
		}
	}
	public override void popUpDisplayed(bool value)
	{
		StoreController.instance.popUpDisplayed (value,gameObject);
	}
	public override void sellCard()
	{
		base.sellCard ();
		StartCoroutine(StoreController.instance.sellCard(gameObject));
	}
	public override void buyXpCard()
	{
		base.buyXpCard ();
		StartCoroutine(StoreController.instance.buyXpCard(gameObject));
	}
	public override void renameCard()
	{
		string tempString = base.renameCardSyntaxCheck ();
		if(tempString!="")
		{
			base.renameCard ();
			StartCoroutine(StoreController.instance.renameCard(tempString,gameObject));
		}
	}
	public override void putOnMarketCard()
	{
		int tempInt = base.putOnMarketSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.putOnMarketCard();
			StartCoroutine(StoreController.instance.putOnMarketCard(tempInt,gameObject));
		}
	}
	public override void editSellPriceCard()
	{
		int tempInt = base.editSellPriceSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.editSellPriceCard();
			StartCoroutine(StoreController.instance.editSellPriceCard(tempInt, gameObject));
		}
	}
	public void setExitButtonsGui(bool value)
	{
		focusStoreFeaturesView.focusStoreFeaturesVM.exitButtonEnabled = value;
	}
	public override void unsellCard()
	{
		base.unsellCard();
		StartCoroutine(StoreController.instance.unsellCard(gameObject));
	}
	public void setIsTutorialLaunched(bool value)
	{
		this.isTutorialLaunched = value;
	}
}

