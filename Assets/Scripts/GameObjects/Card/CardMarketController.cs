using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardMarketController : CardController {

	
	private MarketFeaturesView marketFeaturesView;
	private FocusMarketFeaturesView focusMarketFeaturesView;
	
	void OnMouseOver()
	{
		if (Input.GetMouseButton(1)) 
		{
			MarketController.instance.clickedCard (gameObject);
		}
	}
	public void setMarketCard(Card c)
	{
		base.setCard (c);
		if(c.onSale==0)
		{
			base.applySoldTexture();
		}
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setMarketFeatures ();
	}
	public void resetMarketCard(Card c)
	{
		this.eraseCard ();
		this.setMarketCard (c);
	}
	public void setFocusedMarketCard(Card c)
	{
		base.setCard (c);
		if(c.onSale==0)
		{
			base.applySoldTexture();
		}
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setFocusMarketFeatures ();
	}
	public void resetFocusedMarketCard(Card c)
	{
		this.eraseCard ();
		this.setFocusedMarketCard (c);
	}
	public void setMarketFeatures()
	{
		this.marketFeaturesView = gameObject.AddComponent <MarketFeaturesView>();
		marketFeaturesView.marketFeaturesVM.price = base.card.Price;
		marketFeaturesView.marketFeaturesVM.usernameOwner = base.card.UsernameOwner;
		marketFeaturesView.marketFeaturesVM.onSale = System.Convert.ToBoolean (base.card.onSale);
		marketFeaturesView.marketFeaturesVM.styles=new GUIStyle[ressources.marketFeaturesStyles.Length];
		for(int i=0;i<ressources.marketFeaturesStyles.Length;i++)
		{
			marketFeaturesView.marketFeaturesVM.styles[i]=ressources.marketFeaturesStyles[i];
		}
		marketFeaturesView.marketFeaturesVM.initStyles();
		this.marketFeaturesResize ();
	}
	public override void eraseCard()
	{
		base.eraseCard ();
		if(focusMarketFeaturesView!=null)
		{
			Destroy (this.focusMarketFeaturesView);
		}
		if(marketFeaturesView!=null)
		{
			Destroy (this.marketFeaturesView);
		}
	}
	public void marketFeaturesResize()
	{
		marketFeaturesView.marketFeaturesVM.rect = new Rect(base.GOPosition.x-base.GOSize.x/2f,(Screen.height-base.GOPosition.y)+base.GOSize.y/2f,base.GOSize.x,base.GOSize.y/3);
		marketFeaturesView.marketFeaturesVM.resize(base.GOSize.y);
	}
	public void setFocusMarketFeatures()
	{
		this.focusMarketFeaturesView = gameObject.AddComponent<FocusMarketFeaturesView> ();
		this.focusMarketFeaturesView.focusMarketFeaturesVM.usernameOwner = base.card.UsernameOwner;
		this.focusMarketFeaturesView.focusMarketFeaturesVM.price = base.card.Price;
		this.focusMarketFeaturesView.focusMarketFeaturesVM.nbWin = base.card.nbWin;
		this.focusMarketFeaturesView.focusMarketFeaturesVM.nbLoose = base.card.nbLoose;
		this.focusMarketFeaturesView.focusMarketFeaturesVM.onSale = System.Convert.ToBoolean (base.card.onSale);
		focusMarketFeaturesView.cardFeaturesFocusVM.styles=new GUIStyle[ressources.cardFeaturesFocusStyles.Length];
		for(int i=0;i<ressources.cardFeaturesFocusStyles.Length;i++)
		{
			focusMarketFeaturesView.cardFeaturesFocusVM.styles[i]=ressources.cardFeaturesFocusStyles[i];
		}
		focusMarketFeaturesView.cardFeaturesFocusVM.initStyles();
		this.focusMarketFeaturesResize ();
	}
	public void focusMarketFeaturesResize()
	{
		focusMarketFeaturesView.focusMarketFeaturesVM.cardFeaturesFocusRects[0]=base.getCardFeaturesFocusRect(0);
		focusMarketFeaturesView.focusMarketFeaturesVM.cardFeaturesFocusRects[1]=base.getCardFeaturesFocusRect(1);
		focusMarketFeaturesView.focusMarketFeaturesVM.cardFeaturesFocusRects[2]=base.getCardFeaturesFocusRect(5);
		focusMarketFeaturesView.cardFeaturesFocusVM.resize(base.getCardFeaturesFocusRect(0).height);
	}
	public override void resize()
	{
		base.resize ();
		if(this.marketFeaturesView!=null)
		{                                                            
			this.marketFeaturesResize();
		}
		if(this.focusMarketFeaturesView!=null)
		{                                                            
			this.focusMarketFeaturesResize();
		}
	}
	public override void exitFocus()
	{
		MarketController.instance.exitCard ();
	}
	public override void setGUI(bool value)
	{
		MarketController.instance.setGUI (value);
	}
	public override void setMyGUI(bool value)
	{
		if(this.focusMarketFeaturesView!=null)
		{
			focusMarketFeaturesView.cardFeaturesFocusVM.guiEnabled=value;
		}
		if(this.marketFeaturesView!=null)
		{                                                            
			marketFeaturesView.marketFeaturesVM.guiEnabled=value;
		}
	}
	public override void popUpDisplayed(bool value)
	{
		MarketController.instance.popUpDisplayed (value, gameObject);
	}
	public override void buyCard()
	{
		base.buyCard ();
		StartCoroutine(MarketController.instance.buyCard(gameObject));
	}
}

