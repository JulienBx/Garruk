using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketCardsViewModel {
	
	public GUIStyle[] styles;
	public List<int> cardsToBeDisplayed;
	public int nbCardsPerRow;
	public int nbCardsToDisplay;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int nbCards;
	public GUIStyle[] paginatorGuiStyle;
	public bool newCardsToDisplay;
	public string newCardsLabel;
	public GUIStyle newCardsButtonStyle;
	public GUIStyle newCardsLabelStyle;
	public GUIStyle nbCardsLabelStyle;
	
	public MarketCardsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.cardsToBeDisplayed = new List<int> ();
		this.paginatorGuiStyle=new GUIStyle[0];
	}
	public void initStyles()
	{
		this.newCardsButtonStyle = this.styles [0];
		this.nbCardsLabelStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.newCardsButtonStyle.fontSize = heightScreen * 2 / 100;;
		this.nbCardsLabelStyle.fontSize = heightScreen * 15 / 1000;
	}
}