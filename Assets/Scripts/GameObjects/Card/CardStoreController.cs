using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class CardStoreController : CardController
{

	private FocusStoreFeaturesView focusStoreFeaturesView;

	public CardStoreController  ()
	{
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
		focusStoreFeaturesView.focusStoreFeaturesVM.cardLevel = base.card.getXpLevel();
		focusStoreFeaturesView.focusStoreFeaturesVM.nextLevelCost = base.card.getPriceForNextLevel();
	}
	public void setFocusStoreFeatures()
	{
		this.focusStoreFeaturesView = gameObject.AddComponent<FocusStoreFeaturesView> ();
		focusStoreFeaturesView.focusStoreFeaturesVM.renameCost = base.card.RenameCost;
		focusStoreFeaturesView.focusStoreFeaturesVM.isOnSale = System.Convert.ToBoolean (base.card.onSale);
		focusStoreFeaturesView.focusStoreFeaturesVM.price = base.card.Price;
		focusStoreFeaturesView.focusStoreFeaturesVM.cardCost = base.card.getCost ();
		focusStoreFeaturesView.focusStoreFeaturesVM.cardLevel = base.card.getXpLevel();
		focusStoreFeaturesView.focusStoreFeaturesVM.nextLevelCost = base.card.getPriceForNextLevel();
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
		}
	}
	public override void popUpDisplayed(bool value)
	{
		StoreController.instance.popUpDisplayed (value);
	}
	public override void sellCard()
	{
		base.sellCard ();
		StartCoroutine(StoreController.instance.sellCard());
	}
	public override void buyXpCard()
	{
		base.buyXpCard ();
		StartCoroutine(StoreController.instance.buyXpCard());
	}
	public override void renameCard()
	{
		string tempString = base.renameCardSyntaxCheck ();
		if(tempString!="")
		{
			base.renameCard ();
			StartCoroutine(StoreController.instance.renameCard(tempString));
		}
	}
	public override void putOnMarketCard()
	{
		int tempInt = base.putOnMarketSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.putOnMarketCard();
			StartCoroutine(StoreController.instance.putOnMarketCard(tempInt));
		}
	}
	public override void editSellPriceCard()
	{
		int tempInt = base.editSellPriceSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.editSellPriceCard();
			StartCoroutine(StoreController.instance.editSellPriceCard(tempInt));
		}
	}
	public override void unsellCard()
	{
		base.unsellCard();
		StartCoroutine(StoreController.instance.unsellCard());
	}
}

