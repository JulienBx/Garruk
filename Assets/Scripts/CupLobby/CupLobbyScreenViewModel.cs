using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class CupLobbyScreenViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle blockBackgroundStyle;
	public float gapBetweenBlocks;
	public float blockTopLeftWidth;
	public float blockTopLeftHeight;
	public float blockTopRightWidth;
	public float blockTopRightHeight;
	public float blockMiddleRightWidth;
	public float blockMiddleRightHeight;
	public float blockBottomWidth;
	public float blockBottomHeight;
	public Rect blockTopLeft;
	public Rect blockTopRight;
	public Rect blockMiddleRight;
	public Rect blockBottom;
	public int heightScreen;
	public int widthScreen;
	
	public CupLobbyScreenViewModel ()
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
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.gapBetweenBlocks = 5;
		this.blockBottomWidth = 1f * (this.widthScreen - 2 * this.gapBetweenBlocks);
		this.blockBottomHeight = 0.34f * (0.9f*this.heightScreen - 3 * this.gapBetweenBlocks);
		this.blockTopLeftWidth = 0.75f * (this.widthScreen - 3 * this.gapBetweenBlocks);
		this.blockTopLeftHeight = 0.66f * (0.9f*this.heightScreen - 3 * this.gapBetweenBlocks);
		this.blockTopRightWidth = 0.25f * (this.widthScreen - 4 * this.gapBetweenBlocks);
		this.blockTopRightHeight = 0.25f * (0.9f*this.heightScreen - 4 * this.gapBetweenBlocks-this.blockBottomHeight);
		this.blockMiddleRightWidth = this.blockTopRightWidth;
		this.blockMiddleRightHeight=0.75f * (0.9f * this.heightScreen - 4 * this.gapBetweenBlocks-this.blockBottomHeight);
		
		this.blockTopLeft = new Rect (this.gapBetweenBlocks, 
		                              0.1f * this.heightScreen + this.gapBetweenBlocks, 
		                              this.blockTopLeftWidth, 
		                              this.blockTopLeftHeight);
		
		this.blockTopRight = new Rect (this.blockTopLeftWidth + 2 * this.gapBetweenBlocks,
		                               0.1f * this.heightScreen + this.gapBetweenBlocks,
		                               this.blockTopRightWidth,
		                               this.blockTopRightHeight);
		
		this.blockMiddleRight = new Rect (this.blockTopLeftWidth + 2 * this.gapBetweenBlocks,
		                                  0.1f * this.heightScreen + 2 * this.gapBetweenBlocks + this.blockTopRightHeight,
		                                  this.blockMiddleRightWidth,
		                                  this.blockMiddleRightHeight);
		
		this.blockBottom = new Rect (this.gapBetweenBlocks,
		                             0.1f * this.heightScreen + 2 * this.gapBetweenBlocks + this.blockTopLeftHeight,
		                             this.blockBottomWidth,
		                             this.blockBottomHeight);
	}
}