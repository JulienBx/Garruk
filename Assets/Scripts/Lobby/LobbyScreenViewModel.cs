using System;
using UnityEngine;

public class LobbyScreenViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle blockBackgroundStyle;
	public int heightScreen;
	public int widthScreen;
	public float gapBetweenblocks;
	public float blockTopCenterWidth;
	public float blockTopCenterHeight;
	public float blockMiddleLeftWidth;
	public float blockMiddleLeftHeight;
	public float blockMiddleCenterWidth;
	public float blockMiddleCenterHeight;
	public float blockMiddleRightWidth;
	public float blockMiddleRightHeight;
	public float blockBottomHeight;
	public float blockBottomWidth;
	public Rect blockTopCenter;
	public Rect blockMiddleLeft;
	public Rect blockMiddleCenter;
	public Rect blockMiddleRight;
	public Rect blockBottom;
	public Rect centralWindow;
	public Rect collectionPointsWindow;
	public Rect newSkillsWindow;
	
	public LobbyScreenViewModel ()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.styles=new GUIStyle[0];
		this.blockBackgroundStyle = new GUIStyle ();
		
	}
	public void initStyles()
	{
		this.blockBackgroundStyle = this.styles [0];
	}
	public void resize()
	{
		heightScreen=Screen.height;
		widthScreen=Screen.width;
		
		this.gapBetweenblocks = 5;

		this.blockTopCenterHeight=0.30f * (0.9f * this.heightScreen - 5 * this.gapBetweenblocks);
		this.blockTopCenterWidth=1f *(this.widthScreen - 2*this.gapBetweenblocks);
		this.blockMiddleLeftHeight=0.65f * (0.9f * this.heightScreen - 5 * this.gapBetweenblocks);
		this.blockMiddleLeftWidth=1f/3f *(this.widthScreen - 4*this.gapBetweenblocks);
		this.blockMiddleCenterHeight = blockMiddleLeftHeight;
		this.blockMiddleCenterWidth=blockMiddleLeftWidth;
		this.blockMiddleRightHeight = blockMiddleLeftHeight;
		this.blockMiddleRightWidth=blockMiddleLeftWidth;
		this.blockBottomHeight=0.05f * (0.9f * this.heightScreen - 5 * this.gapBetweenblocks);
		this.blockBottomWidth=1f *(this.widthScreen - 2*this.gapBetweenblocks);

		this.blockTopCenter = new Rect (this.gapBetweenblocks,
		                              0.1f * this.heightScreen + this.gapBetweenblocks, 
		                              this.blockTopCenterWidth,
		                              this.blockTopCenterHeight);

		this.blockMiddleLeft = new Rect (this.gapBetweenblocks,
		                                0.1f * this.heightScreen + 3*this.gapBetweenblocks+this.blockTopCenterHeight,
		                                this.blockMiddleLeftWidth,
		                                this.blockMiddleLeftHeight);

		this.blockMiddleCenter = new Rect (this.blockMiddleLeftWidth+2*this.gapBetweenblocks,
		                                   0.1f * this.heightScreen + 3*this.gapBetweenblocks+this.blockTopCenterHeight,
		                                   this.blockMiddleCenterWidth,
		                                   this.blockMiddleCenterHeight);

		this.blockMiddleRight = new Rect (this.blockMiddleLeftWidth+this.blockMiddleCenterWidth+3*this.gapBetweenblocks,
		                                   0.1f * this.heightScreen + 3*this.gapBetweenblocks+this.blockTopCenterHeight,
		                                   this.blockMiddleRightWidth,
		                                   this.blockMiddleRightHeight);

		this.blockBottom = new Rect (this.gapBetweenblocks,
		                             0.1f * this.heightScreen + 4*this.gapBetweenblocks+this.blockTopCenterHeight+this.blockMiddleCenterHeight,
		                             this.blockBottomWidth,
		                             this.blockBottomHeight);
		
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

