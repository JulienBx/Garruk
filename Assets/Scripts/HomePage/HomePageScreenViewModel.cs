using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageScreenViewModel {
	
	public GUIStyle[] styles;
	public GUIStyle blockBorderStyle;

	public int heightScreen;
	public int widthScreen;
	public float gapBetweenblocks;
	public float blockTopLeftHeight;
	public float blockTopLeftWidth;
	public float blockBottomLeftHeight;
	public float blockBottomLeftWidth;
	public float blockTopRightHeight;
	public float blockTopRightWidth;
	public float blockMiddleRightHeight;
	public float blockMiddleRightWidth;
	public float blockBottomRightHeight;
	public float blockBottomRightWidth;

	public Rect blockTopLeft;
	public Rect blockBottomLeft;
	public Rect blockTopRight;
	public Rect blockMiddleRight;
	public Rect blockBottomRight;


	public HomePageScreenViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.blockBorderStyle = new GUIStyle ();
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.blockTopLeft = new Rect ();
		this.blockBottomLeft = new Rect ();
		this.blockTopRight = new Rect ();
		this.blockMiddleRight = new Rect ();
		this.blockBottomRight = new Rect ();
	}
	public void initStyles()
	{	
		this.blockBorderStyle = this.styles [0];
	}
	public void resize()
	{	
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		
		this.gapBetweenblocks = 5;

		this.blockTopLeftWidth = 0.66f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockTopLeftHeight = 0.40f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);

		this.blockBottomLeftWidth = 0.66f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockBottomLeftHeight = 0.60f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);

		this.blockTopRightWidth = 0.34f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockTopRightHeight = 0.30f * (0.9f * this.heightScreen - 4 * this.gapBetweenblocks);
		
		this.blockMiddleRightWidth = 0.34f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockMiddleRightHeight = 0.30f * (0.9f * this.heightScreen - 4 * this.gapBetweenblocks);

		this.blockBottomRightWidth = 0.34f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockBottomRightHeight = 0.40f * (0.9f * this.heightScreen - 4 * this.gapBetweenblocks);

		this.blockTopLeft = new Rect (this.gapBetweenblocks,
		                              0.1f * this.heightScreen + this.gapBetweenblocks,
		                              this.blockTopLeftWidth,
		                              this.blockTopLeftHeight);

		this.blockBottomLeft = new Rect (this.gapBetweenblocks,
		                                 0.1f * this.heightScreen + 2*this.gapBetweenblocks+this.blockTopLeftHeight,
		                                 this.blockBottomLeftWidth,
		                                 this.blockBottomLeftHeight);

		this.blockTopRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks,
		                               0.1f * this.heightScreen+ this.gapBetweenblocks,
		                               this.blockTopRightWidth,
		                               this.blockTopRightHeight);

		this.blockMiddleRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 
		                                  0.1f * this.heightScreen+2*this.gapBetweenblocks+this.blockTopRightHeight,
		                                  this.blockMiddleRightWidth, 
		                                  this.blockMiddleRightHeight);

		this.blockBottomRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 
		                                  0.1f * this.heightScreen+3*this.gapBetweenblocks+this.blockMiddleRightHeight+this.blockTopRightHeight, 
		                                  this.blockBottomRightWidth, 
		                                  this.blockBottomRightHeight);
	}
}