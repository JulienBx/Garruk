using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameCardsViewModel
{

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


	public IList<Card> CardStoreController;
	
	public MyGameCardsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.cardsToBeDisplayed = new List<int> ();
		this.paginatorGuiStyle=new GUIStyle[0];
	}
	public void initStyles()
	{

	}
	public void resize(int heightScreen)
	{

	}
	

}
