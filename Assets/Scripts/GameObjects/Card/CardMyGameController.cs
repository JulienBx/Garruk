using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardMyGameController : CardController {

	private FocusMyGameFeaturesView focusMyGameFeaturesView;
	
	
	void OnMouseOver()
	{
		if (Input.GetMouseButton(1)) 
		{
			//MyGameController.instance.rightClickedCard (gameObject.name); Fonction pour signaler à la scène qu'un clic droit a été opéré sur la carte
		}
	}
	void OnMouseClick()
	{
		//MyGameController.instance.clickedCard (gameObject.name); Fonction pour signaler à la scène qu'un clic gauche a été opéré sur la carte

	}
	public void setFocusMyGameFeatures(bool canBeSold=true)
	{
		this.focusMyGameFeaturesView = gameObject.AddComponent<FocusMyGameFeaturesView> ();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.canBeSold = canBeSold;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.renameCost = base.card.RenameCost;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbWin = base.card.nbWin;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nbLoose = base.card.nbLoose;
		this.updateVM ();
		focusMyGameFeaturesView.cardFeaturesFocusVM.styles=new GUIStyle[ressources.cardFeaturesFocusStyles.Length];
		for(int i=0;i<ressources.cardFeaturesFocusStyles.Length;i++)
		{
			focusMyGameFeaturesView.cardFeaturesFocusVM.styles[i]=ressources.cardFeaturesFocusStyles[i];
		}
		focusMyGameFeaturesView.cardFeaturesFocusVM.initStyles();
		this.focusMyGameFeaturesResize ();
	}
	public override void updateVM ()
	{
		focusMyGameFeaturesView.focusMyGameFeaturesVM.isOnSale = System.Convert.ToBoolean (base.card.onSale);
		focusMyGameFeaturesView.focusMyGameFeaturesVM.price = base.card.Price;
		focusMyGameFeaturesView.focusMyGameFeaturesVM.cardCost = base.card.getCost ();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.cardLevel = base.card.getXpLevel();
		focusMyGameFeaturesView.focusMyGameFeaturesVM.nextLevelCost = base.card.getPriceForNextLevel();
	}
	public void focusMyGameFeaturesResize()
	{
		for(int i=0;i<6;i++)
		{
			focusMyGameFeaturesView.focusMyGameFeaturesVM.cardFeaturesFocusRects[i]=base.getCardFeaturesFocusRect(i);
		}
		focusMyGameFeaturesView.cardFeaturesFocusVM.resize(base.getCardFeaturesFocusRect(0).height);
	}
	public override void resize()
	{
		base.resize ();
		if(this.focusMyGameFeaturesView!=null)
		{                                                            
			this.focusMyGameFeaturesResize();
		}
	}
	public override void hideFeatures()
	{
		if(focusMyGameFeaturesView!=null)
		{
			Destroy (this.focusMyGameFeaturesView);
		}
	}
	public void exitFocus()
	{
		if(focusMyGameFeaturesView!=null)
		{
			Destroy (this.focusMyGameFeaturesView);
		}
		//MyGameController.instance.exitCard (gameObject.name); Fonction pour détecter qu'on quitte le zoom sur une carte
	}
	public override void setGUI(bool value)
	{
		if(this.focusMyGameFeaturesView!=null)
		{
			this.focusMyGameFeaturesView.cardFeaturesFocusVM.guiEnabled=value;
		}
	}
	public override void popUpDisplayed(bool value)
	{
		//MyGameController.instance.popUpdisplayed (value, gameObject.name); Fonction pour indiquer à la scène qu'une popup est affichée / ou masquée
		
	}
	public override void hideCard()
	{
		if(focusMyGameFeaturesView!=null)
		{
			Destroy (this.focusMyGameFeaturesView);
		}
		//MyGameController.instance.hideCard (gameObject.name);	Fonction pour indiquer à la scène qu'une carte doit disparaitre (par exemple si elle a été désintégrée)
	}
	public override IEnumerator sellCard()
	{
		StartCoroutine(base.sellCard ());
		yield break;
	}
	public override IEnumerator buyXpCard()
	{
		StartCoroutine(base.buyXpCard ());
		yield break;
	}
	public override IEnumerator renameCard()
	{
		StartCoroutine(base.renameCard ());
		yield break;
	}
	public override IEnumerator putOnMarketCard()
	{
		StartCoroutine(base.putOnMarketCard ());
		yield break;
	}
	public override IEnumerator editSellPriceCard()
	{
		StartCoroutine(base.editSellPriceCard ());
		yield break;
	}
	public override IEnumerator unsellCard()
	{
		StartCoroutine(base.unsellCard ());
		yield break;
	}
}

