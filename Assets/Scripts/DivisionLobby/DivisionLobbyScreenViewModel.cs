using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DivisionLobbyScreenViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle blockBackgroundStyle;
	public float gapBetweenBlocks;
	public float blockTopLeftWidth;
	public float blockTopLeftHeight;
	public float blockTopRightWidth;
	public float blockTopRightHeight;
	public float blockBottomLeftWidth;
	public float blockBottomLeftHeight;
	public float blockMiddleRightWidth;
	public float blockMiddleRightHeight;
	public float blockBottomRightWidth;
	public float blockBottomRightHeight;
	public Rect blockTopLeft;
	public Rect blockTopRight;
	public Rect blockBottomLeft;
	public Rect blockMiddleRight;
	public Rect blockBottomRight;
	public int heightScreen;
	public int widthScreen;
	
	public DivisionLobbyScreenViewModel ()
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
		this.blockTopLeftWidth = 0.75f * (this.widthScreen - 3 * this.gapBetweenBlocks);
		this.blockTopLeftHeight = 0.66f * (0.9f*this.heightScreen - 3 * this.gapBetweenBlocks);
		this.blockTopRightWidth = 0.25f * (this.widthScreen - 3 * this.gapBetweenBlocks);
		this.blockTopRightHeight = 0.15f * (0.9f*this.heightScreen - 4 * this.gapBetweenBlocks);
		this.blockBottomLeftWidth = this.blockTopLeftWidth;
		this.blockBottomLeftHeight = 0.34f * (0.9f * this.heightScreen - 3 * this.gapBetweenBlocks);
		this.blockMiddleRightWidth = this.blockTopRightWidth;
		this.blockMiddleRightHeight=0.70f * (0.9f * this.heightScreen - 4 * this.gapBetweenBlocks);
		this.blockBottomRightWidth = this.blockTopRightWidth;
		this.blockBottomRightHeight = this.blockTopRightHeight;

		this.blockTopLeft = new Rect (this.gapBetweenBlocks, 
		                              0.1f * this.heightScreen + this.gapBetweenBlocks, 
		                              this.blockBottomLeftWidth, 
		                              this.blockTopLeftHeight);

		this.blockTopRight = new Rect (this.blockTopLeftWidth + 2 * this.gapBetweenBlocks,
		                               0.1f * this.heightScreen + this.gapBetweenBlocks,
		                               this.blockTopRightWidth,
		                               this.blockTopRightHeight);

		this.blockBottomLeft = new Rect (this.gapBetweenBlocks,
		                                 0.1f * this.heightScreen + this.blockTopLeftHeight + 2 * this.gapBetweenBlocks,
		                                 this.blockBottomLeftWidth,
		                                 this.blockBottomLeftHeight);

		this.blockMiddleRight = new Rect (this.blockBottomLeftWidth + 2 * this.gapBetweenBlocks,
		                                0.1f * this.heightScreen + 2 * this.gapBetweenBlocks + this.blockTopRightHeight,
		                                this.blockMiddleRightWidth,
		                                this.blockMiddleRightHeight);
		                            
		this.blockBottomRight = new Rect (this.blockBottomLeftWidth + 2 * this.gapBetweenBlocks,
		                                  0.1f * this.heightScreen + 3 * this.gapBetweenBlocks + this.blockTopRightHeight+this.blockMiddleRightHeight,
		                                  this.blockBottomRightWidth,
		                                  this.blockBottomRightHeight);
	}
}