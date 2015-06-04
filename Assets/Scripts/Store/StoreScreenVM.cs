using System;
using UnityEngine;
public class StoreScreenViewModel
{

	public int heightScreen;
	public int widthScreen;
	public float gapBetweenBlocks;
	public float blockMainWidth;
	public float blockMainHeight;
	public Rect blockMain;
	public Rect centralWindow;
	public Rect centralWindowSelectCardType;
	public Rect collectionPointsWindow;
	public Rect newSkillsWindow;


	public StoreScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
	}
	public void resize()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;

		this.gapBetweenBlocks = 5;

		this.blockMainWidth = 1f*this.widthScreen - (2*this.gapBetweenBlocks);
		this.blockMainHeight=1f*(0.9f*this.heightScreen-2*this.gapBetweenBlocks);
		this.blockMain = new Rect (this.gapBetweenBlocks, 0.1f * this.heightScreen + this.gapBetweenBlocks, this.blockMainWidth, this.blockMainHeight);
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.centralWindowSelectCardType = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.45f * this.heightScreen);
		this.collectionPointsWindow = new Rect (this.widthScreen - this.widthScreen * 0.15f-this.gapBetweenBlocks, 
		                                        0.1f * this.heightScreen+this.gapBetweenBlocks, 
		                                        this.widthScreen * 0.15f, 
		                                        this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin,
		                                 this.collectionPointsWindow.yMax + this.gapBetweenBlocks,
		                                 this.collectionPointsWindow.width,
		                                 this.heightScreen - 0.1f * this.heightScreen - 2 * this.gapBetweenBlocks - this.collectionPointsWindow.height);

	}
}

