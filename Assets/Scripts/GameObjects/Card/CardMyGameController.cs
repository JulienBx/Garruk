using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardMyGameController : CardController
{
	private FocusMyGameFeaturesView focusMyGameFeaturesView;
	
	void OnMouseOver()
	{
		if (Input.GetMouseButton(1))
		{
			//myGameController.instance.rightClickedCard(gameObject);
		}
	}
	void OnMouseUpAsButton()
	{
		//myGameController.instance.clickedCard(gameObject); 
	}
	public void setMyGameCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
	}
	public void resetMyGameCard(Card c)
	{
		this.eraseCard ();
		this.setMyGameCard (c);
	}
	public void setFocusedMyGameCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setFocusMyGameFeatures (true);
	}
	public void resetFocusedMyGameCard(Card c)
	{
		this.eraseCard ();
		this.setFocusedMyGameCard (c);
	}
	public void setFocusedMyGameDeckCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setFocusMyGameFeatures (false);
	}
	public void resetFocusedMyGameDeckCard(Card c)
	{
		this.eraseCard ();
		this.setFocusedMyGameCard (c);
	}
	public override void eraseCard()
	{
		base.eraseCard ();
		if(focusMyGameFeaturesView!=null)
		{
			Destroy (this.focusMyGameFeaturesView);
		}
	}
	public override void updateExperience()
	{
		base.updateExperience ();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.cardLevel = base.card.getXpLevel();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nextLevelCost = base.card.getPriceForNextLevel();
	}
	public void setFocusMyGameFeatures(bool canBeSold)
	{
		this.focusMyGameFeaturesView = gameObject.AddComponent<FocusMyGameFeaturesView>();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.canBeSold = canBeSold;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.renameCost = base.card.RenameCost;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbWin = base.card.nbWin;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbLoose = base.card.nbLoose;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.isOnSale = System.Convert.ToBoolean(base.card.onSale);
		focusMyGameFeaturesView.focusMyGameFeaturesVM.price = base.card.Price;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.cardCost = base.card.getCost();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.cardLevel = base.card.getXpLevel();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nextLevelCost = base.card.getPriceForNextLevel();
		focusMyGameFeaturesView.cardFeaturesFocusVM.styles = new GUIStyle[ressources.cardFeaturesFocusStyles.Length];
		for (int i=0; i<ressources.cardFeaturesFocusStyles.Length; i++)
		{
			focusMyGameFeaturesView.cardFeaturesFocusVM.styles [i] = ressources.cardFeaturesFocusStyles [i];
		}
		focusMyGameFeaturesView.cardFeaturesFocusVM.initStyles();
		this.focusMyGameFeaturesResize();
	}
	public void focusMyGameFeaturesResize()
	{
		for (int i=0; i<6; i++)
		{
			focusMyGameFeaturesView.focusMyGameFeaturesVM.cardFeaturesFocusRects [i] = base.getCardFeaturesFocusRect(i);
		}
		focusMyGameFeaturesView.cardFeaturesFocusVM.resize(base.getCardFeaturesFocusRect(0).height);
	}
	public override void resize()
	{
		base.resize();
		if (this.focusMyGameFeaturesView != null)
		{                                                            
			this.focusMyGameFeaturesResize();
		}
	}
	public void exitFocus()
	{
		//myGameController.instance.exitCard();
	}
	public override void setGUI(bool value)
	{
		//MarketController.instance.setGUI (value);
	}
	public override void setMyGUI(bool value)
	{
		if (this.focusMyGameFeaturesView != null)
		{
			this.focusMyGameFeaturesView.cardFeaturesFocusVM.guiEnabled = value;
		}
	}
	public override void popUpDisplayed(bool value)
	{
		//MyGameController.instance.popUpdisplayed (value, gameObject.name); Fonction pour indiquer à la scène qu'une popup est affichée / ou masquée
	}
	public override void hideCard()
	{
		//MyGameController.instance.hideCard (gameObject.name);	Fonction pour indiquer à la scène qu'une carte doit disparaitre (par exemple si elle a été désintégrée)
	}
	public override void sellCard()
	{
		base.sellCard ();
		//StartCoroutine(myGameController.instance.sellCard());
	}
	public override void buyXpCard()
	{
		base.buyXpCard ();
		//StartCoroutine(myGameController.instance.buyXpCard());
	}
	public override void renameCard()
	{
		string tempString = renameCardSyntaxCheck ();
		if(tempString!="")
		{
			base.renameCard ();
			//StartCoroutine(myGameController.instance.renameCard(tempString));
		}
	}
	public override void putOnMarketCard()
	{
		int tempInt = putOnMarketSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.putOnMarketCard();
			//StartCoroutine(myGameController.instance.putOnMarketCard(tempInt));
		}
	}
	public override void editSellPriceCard()
	{
		int tempInt = editSellPriceSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.editSellPriceCard();
			//StartCoroutine(myGameController.instance.editSellPriceCard(tempInt));
		}
	}
	public override void unsellCard()
	{
		base.unsellCard();
		//StartCoroutine(myGameController.instance.unsellCard());
	}
}

