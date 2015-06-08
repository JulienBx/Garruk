using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketScreenViewModel {
	
	public GUIStyle[] styles;
	public int heightScreen;
	public int widthScreen;
	public float gapBetweenblocks;
	public float blockLeftWidth;
	public float blockLeftHeight;
	public float blockRightWidth;
	public float blockRightHeight;
	public Rect blockLeft;
	public Rect blockRight;
	public Rect centralWindow;
	public Rect collectionPointsWindow;
	public Rect newSkillsWindow;
	
	public MarketScreenViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.heightScreen=Screen.height;
		this.widthScreen=Screen.width;
	}
	public void initStyles()
	{
	}
	public void resize()
	{
		heightScreen=Screen.height;
		widthScreen=Screen.width;

		this.gapBetweenblocks = 5;
		
		this.blockLeftHeight=1f*(0.9f*this.heightScreen-2*this.gapBetweenblocks);
		this.blockLeftWidth=0.80f*(this.widthScreen - 3*this.gapBetweenblocks);
		this.blockRightHeight=1f*(0.9f*this.heightScreen-2*this.gapBetweenblocks);
		this.blockRightWidth=0.20f*(this.widthScreen - 3*this.gapBetweenblocks);
		
		this.blockLeft = new Rect (this.gapBetweenblocks, 
		                           0.1f * this.heightScreen + this.gapBetweenblocks, 
		                           this.blockLeftWidth, 
		                           this.blockLeftHeight);
		
		this.blockRight = new Rect (this.blockLeftWidth+2*this.gapBetweenblocks, 
		                                0.1f * this.heightScreen + this.gapBetweenblocks,
		                                this.blockRightWidth, 
		                                this.blockRightHeight);
		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.collectionPointsWindow = new Rect (this.widthScreen - this.widthScreen * 0.15f-this.gapBetweenblocks, 
		                                        0.1f * this.heightScreen+this.gapBetweenblocks, 
		                                        this.widthScreen * 0.15f, 
		                                        this.heightScreen * 0.1f);
		this.newSkillsWindow = new Rect (this.collectionPointsWindow.xMin,
		                                 this.collectionPointsWindow.yMax + this.gapBetweenblocks,
		                                 this.collectionPointsWindow.width,
		                                 this.heightScreen - 0.1f * this.heightScreen - 2 * this.gapBetweenblocks - this.collectionPointsWindow.height);

	}
}