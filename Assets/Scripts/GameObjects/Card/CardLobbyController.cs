using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardLobbyController : CardController
{
	private FocusLobbyFeaturesView focusLobbyFeaturesView;
	
	void OnMouseOver()
	{
		if (Input.GetMouseButton(1)) 
		{
			LobbyController.instance.rightClickedCard (gameObject);
		}
	}
	public void setLobbyCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
	}
	public void resetLobbyCard(Card c)
	{
		this.eraseCard ();
		this.setLobbyCard (c);
	}
	public void setFocusedLobbyCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
		this.setFocusLobbyFeatures ();
	}
	public void resetFocusedLobbyCard(Card c)
	{
		this.eraseCard ();
		this.setFocusedLobbyCard (c);
	}
	public override void eraseCard()
	{
		base.eraseCard ();
		if(focusLobbyFeaturesView!=null)
		{
			Destroy (this.focusLobbyFeaturesView);
		}
	}
	public override void changeDeckOrder(bool moveLeft)
	{
		StartCoroutine(LobbyController.instance.changeDeckOrder(gameObject,moveLeft));
	}
	public override void updateExperience()
	{
		base.updateExperience ();
		focusLobbyFeaturesView.focusLobbyFeaturesVM.cardLevel = base.card.ExperienceLevel;
		focusLobbyFeaturesView.focusLobbyFeaturesVM.nextLevelCost = base.card.NextLevelPrice;
	}
	public void setFocusLobbyFeatures()
	{
		this.focusLobbyFeaturesView = gameObject.AddComponent<FocusLobbyFeaturesView>();
		focusLobbyFeaturesView.focusLobbyFeaturesVM.renameCost = base.card.RenameCost;
		focusLobbyFeaturesView.focusLobbyFeaturesVM.nbWin = base.card.nbWin;
		focusLobbyFeaturesView.focusLobbyFeaturesVM.nbLoose = base.card.nbLoose;
		focusLobbyFeaturesView.focusLobbyFeaturesVM.cardLevel = base.card.ExperienceLevel;
		focusLobbyFeaturesView.focusLobbyFeaturesVM.nextLevelCost = base.card.NextLevelPrice;
		focusLobbyFeaturesView.cardFeaturesFocusVM.styles = new GUIStyle[ressources.cardFeaturesFocusStyles.Length];
		for (int i=0; i<ressources.cardFeaturesFocusStyles.Length; i++)
		{
			focusLobbyFeaturesView.cardFeaturesFocusVM.styles [i] = ressources.cardFeaturesFocusStyles [i];
		}
		focusLobbyFeaturesView.cardFeaturesFocusVM.initStyles();
		this.focusLobbyFeaturesResize();
	}
	public void focusLobbyFeaturesResize()
	{
		for (int i=0; i<3; i++)
		{
			focusLobbyFeaturesView.focusLobbyFeaturesVM.cardFeaturesFocusRects [i] = base.getCardFeaturesFocusRect(i);
		}
		focusLobbyFeaturesView.focusLobbyFeaturesVM.cardFeaturesFocusRects [3] = base.getCardFeaturesFocusRect(5);
		focusLobbyFeaturesView.cardFeaturesFocusVM.resize(base.getCardFeaturesFocusRect(0).height);
	}
	public override void resize()
	{
		base.resize();
		if (this.focusLobbyFeaturesView != null)
		{                                                            
			this.focusLobbyFeaturesResize();
		}
	}
	public void exitFocus()
	{
		LobbyController.instance.exitCard();
	}
	public override void setGUI(bool value)
	{
		LobbyController.instance.setGUI (value);
	}
	public override void setMyGUI(bool value)
	{
		if (this.focusLobbyFeaturesView != null)
		{
			this.focusLobbyFeaturesView.cardFeaturesFocusVM.guiEnabled = value;
		}
	}
	public override void setButtonsGui(bool value)
	{
		base.setButtonsGui (value);
	}
	public override void popUpDisplayed(bool value)
	{
		LobbyController.instance.popUpDisplayed (value, gameObject);
	}	
	public override void buyXpCard()
	{
		base.buyXpCard ();
		StartCoroutine(LobbyController.instance.buyXpCard(gameObject));
	}
	public override void renameCard()
	{
		string tempString = renameCardSyntaxCheck ();
		if(tempString!="")
		{
			base.renameCard ();
			StartCoroutine(LobbyController.instance.renameCard(tempString,gameObject));
		}
	}
}

