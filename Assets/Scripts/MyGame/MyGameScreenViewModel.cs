using System;
using UnityEngine;

public class MyGameScreenViewModel
{

	public GUIStyle[] styles;
	public int heightScreen;
	public int widthScreen;
	public float gapBetweenblocks;
	public float blockCardsWidth;
	public float blockCardsHeight;
	public float blockDecksWidth;
	public float blockDecksHeight;
	public float blockDeckCardsWidth;
	public float blockDeckCardsHeight;
	public float blockFiltersWidth;
	public float blockFiltersHeight;
	public Rect blockCards;
	public Rect blockDecks;
	public Rect blockDeckCards;
	public Rect blockFilters;
	public Rect centralWindow;

	public MyGameScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;

	}
	public void initStyles()
	{
		this.styles=new GUIStyle[0];	
	}
	public void resize()
	{
		heightScreen=Screen.height;
		widthScreen=Screen.width;
		
		this.gapBetweenblocks = 5;

		this.blockCardsHeight = 0.75f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockCardsWidth = 0.80f*(this.widthScreen - 3*this.gapBetweenblocks);
		this.blockDeckCardsHeight = 0.25f*(0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockDeckCardsWidth = 0.75f*0.80f*(this.widthScreen - 4*this.gapBetweenblocks);
		this.blockDecksHeight = 0.25f*(0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockDecksWidth = 0.25f*0.80f*(this.widthScreen - 4*this.gapBetweenblocks);
		this.blockFiltersHeight =0.9f * this.heightScreen - 2 * this.gapBetweenblocks;
		this.blockFiltersWidth = 0.20f*(this.widthScreen - 3*this.gapBetweenblocks);

		
		this.blockCards = new Rect (this.gapBetweenblocks, 
		                           0.1f * this.heightScreen + 2*this.gapBetweenblocks+this.blockDeckCardsHeight, 
		                           this.blockCardsWidth, 
		                           this.blockCardsHeight);
		
		this.blockDeckCards = new Rect (this.blockDecksWidth+2*this.gapBetweenblocks,
		                                0.1f * this.heightScreen + this.gapBetweenblocks,
		                                this.blockDeckCardsWidth,
		                                this.blockDeckCardsHeight);

		this.blockDecks = new Rect (this.gapBetweenblocks, 
		                            0.1f * this.heightScreen + this.gapBetweenblocks,
		                            this.blockDecksWidth,
		                            this.blockDecksHeight);

		this.blockFilters = new Rect (this.blockCardsWidth+2*this.gapBetweenblocks,
		                              0.1f * this.heightScreen + this.gapBetweenblocks,
		                              this.blockFiltersWidth,
		                              this.blockFiltersHeight);

		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
	}
}

