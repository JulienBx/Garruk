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
			MyGameController.instance.rightClickedCard (gameObject);
		}
	}
	void OnMouseDown()
	{
		MyGameController.instance.leftClickedCard(gameObject);
	}
	public void setMyGameCard(Card c)
	{
		base.setCard (c);
		if(c.onSale==0 && c.IdOWner==-1)
		{
			base.applySoldTexture();
		}
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
		if(c.onSale==0 && c.IdOWner==-1)
		{
			base.applySoldTexture();
		}
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setFocusMyGameFeatures ();
	}
	public void resetFocusedMyGameCard(Card c)
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
	public void setFocusMyGameFeatures()
	{
		this.focusMyGameFeaturesView = gameObject.AddComponent<FocusMyGameFeaturesView>();
		if(base.card.Decks.Count>0)
		{
			focusMyGameFeaturesView.focusMyGameFeaturesVM.canBeSold=false;
		}
		else
		{
			focusMyGameFeaturesView.focusMyGameFeaturesVM.canBeSold=true;
		}
		focusMyGameFeaturesView.focusMyGameFeaturesVM.renameCost = base.card.RenameCost;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbWin = base.card.nbWin;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbLoose = base.card.nbLoose;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.isOnSale = System.Convert.ToBoolean(base.card.onSale);
		focusMyGameFeaturesView.focusMyGameFeaturesVM.idOwner = base.card.IdOWner;
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
		MyGameController.instance.exitCard();
	}
	public override void setGUI(bool value)
	{
		MyGameController.instance.setGUI (value);
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
		MyGameController.instance.popUpDisplayed (value, gameObject);
	}
	public override void sellCard()
	{
		base.sellCard ();
		StartCoroutine(MyGameController.instance.sellCard(gameObject));
	}
	public override void buyXpCard()
	{
		base.buyXpCard ();
		StartCoroutine(MyGameController.instance.buyXpCard(gameObject));
	}
	public override void renameCard()
	{
		string tempString = renameCardSyntaxCheck ();
		if(tempString!="")
		{
			base.renameCard ();
			StartCoroutine(MyGameController.instance.renameCard(tempString,gameObject));
		}
	}
	public override void putOnMarketCard()
	{
		int tempInt = putOnMarketSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.putOnMarketCard();
			StartCoroutine(MyGameController.instance.putOnMarketCard(tempInt,gameObject));
		}
	}
	public override void editSellPriceCard()
	{
		int tempInt = editSellPriceSyntaxCheck ();
		if(tempInt!=-1)
		{
			base.editSellPriceCard();
			StartCoroutine(MyGameController.instance.editSellPriceCard(tempInt,gameObject));
		}
	}
	public override void unsellCard()
	{
		base.unsellCard();
		StartCoroutine(MyGameController.instance.unsellCard(gameObject));
	}
}

