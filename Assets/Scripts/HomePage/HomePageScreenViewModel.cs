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
	public float blockLeftHeight;
	public float blockLeftWidth;
	public float blockRightHeight;
	public float blockRightWidth;

	public Rect blockLeft;
	public Rect blockRight;


	public HomePageScreenViewModel ()
	{
		this.styles = new GUIStyle[0];
		this.blockBorderStyle = new GUIStyle ();
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.blockLeft = new Rect ();
		this.blockRight = new Rect ();
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

		this.blockLeftWidth = 0.5f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockLeftHeight = 1f * (0.9f * this.heightScreen - 2 * this.gapBetweenblocks);

		this.blockRightWidth = 0.5f * (this.widthScreen - 3 * this.gapBetweenblocks);
		this.blockRightHeight = 1f * (0.9f * this.heightScreen - 2 * this.gapBetweenblocks);
		
		this.blockLeft = new Rect (this.gapBetweenblocks, 
		                           0.1f * this.heightScreen + this.gapBetweenblocks, 
		                           this.blockLeftWidth, 
		                           this.blockLeftHeight);

		this.blockRight = new Rect (2*this.gapBetweenblocks+this.blockLeftWidth, 
		                           0.1f * this.heightScreen + this.gapBetweenblocks, 
		                           this.blockRightWidth, 
		                           this.blockRightHeight);
	}
}