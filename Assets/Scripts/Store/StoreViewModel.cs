using System;
using UnityEngine;

public class StoreViewModel
{

	public GUIStyle[] styles;
	public GUIStyle button1CardStyle;
	public GUIStyle button5CardStyle;
	public GUIStyle button1CardWithCardTypeStyle;
	public GUIStyle button5CardWithCardTypeStyle;
	public GUIStyle buttonAddCreditsStyle;
	public GUIStyle titleStyle;
	public GUIStyle buttonStyle;
	public bool guiEnabled;
	public bool isPopUpDisplayed;
	public bool hideGUI;
	public bool are5CardsDisplayed;
	public bool canAddCredits;
	public int cardCreationCost;
	public int fiveCardsCreationCost;
	public int cardWithCardTypeCreationCost;
	public int fiveCardsWidthCardTypeCreationCost;
	public string title;
	public string noCardTypes;

	public StoreViewModel ()
	{
		this.guiEnabled = true;
		this.isPopUpDisplayed = false;
		this.hideGUI = false;
		this.canAddCredits = false;
		this.button1CardStyle = new GUIStyle ();
		this.button5CardStyle = new GUIStyle ();
		this.button1CardWithCardTypeStyle = new GUIStyle ();
		this.button5CardWithCardTypeStyle = new GUIStyle ();
		this.buttonAddCreditsStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.cardCreationCost = 500;
		this.fiveCardsCreationCost = 2000;
		this.cardWithCardTypeCreationCost = 1000;
		this.fiveCardsWidthCardTypeCreationCost = 4000;
	}
	public void initStyles()
	{
		this.button1CardStyle = this.styles [0];
		this.button5CardStyle = this.styles [1];
		this.button1CardWithCardTypeStyle = this.styles [2];
		this.button5CardWithCardTypeStyle = this.styles [3];
		this.buttonAddCreditsStyle = this.styles [4];
		this.titleStyle = this.styles [5];
		this.buttonStyle = this.styles [6];
	}
	public void resize(int heightSreen)
	{
		this.button1CardStyle.fontSize = heightSreen * 3 / 100;
		this.button5CardStyle.fontSize = heightSreen * 3 / 100;
		this.button1CardWithCardTypeStyle.fontSize = heightSreen * 3 / 100;
		this.button5CardWithCardTypeStyle.fontSize = heightSreen * 3 / 100;
		this.buttonAddCreditsStyle.fontSize = heightSreen * 3 / 100;
		this.titleStyle.fontSize = heightSreen * 3 / 100;
	}
	
}
