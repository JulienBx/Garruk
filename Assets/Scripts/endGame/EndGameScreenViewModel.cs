using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScreenViewModel 
{
	public int heightScreen;
	public int widthScreen;

	public float blockTopLeftWidth;
	public float blockTopLeftHeight;
	public float blockTopRightWidth;
	public float blockTopRightHeight;
	public float blockMiddleRightWidth;
	public float blockMiddleRightHeight;
	public float blockBottomRightWidth;
	public float blockBottomRightHeight;
	public float blockBottomWidth;
	public float blockBottomHeight;
	public float gapBetweenblocks;

	public Rect blockTopLeft;
	public Rect blockTopRight;
	public Rect blockBottom;
	public Rect blockMiddleRight;
	public Rect blockBottomRight;

	public Rect centralWindow;

	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle blockBorderStyle;
	
	public EndGameScreenViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.centralWindowStyle = new GUIStyle ();
		this.centralWindowTitleStyle = new GUIStyle ();
		this.centralWindowButtonStyle = new GUIStyle ();
		this.blockBorderStyle = new GUIStyle ();
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
	}
	public void initStyles ()
	{
		this.centralWindowStyle=this.styles[0];
		this.centralWindowTitleStyle=this.styles[1];
		this.centralWindowButtonStyle=this.styles[2];
		this.blockBorderStyle=this.styles[3];
	}
	public void resize()
	{
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;
		this.gapBetweenblocks = 0.005f * this.widthScreen;
		this.blockTopLeftHeight = 0.66f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		this.blockTopLeftWidth = 0.75f*(this.widthScreen-3*this.gapBetweenblocks);
		this.blockBottomHeight = 0.34f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		this.blockBottomWidth = 1f * (this.widthScreen - 2 * this.gapBetweenblocks);
		this.blockTopRightWidth = 0.25f*(this.widthScreen-3*this.gapBetweenblocks);
		this.blockTopRightHeight = 0.15f*(0.9f*this.heightScreen-5*this.gapBetweenblocks-this.blockBottomHeight);
		this.blockMiddleRightWidth = this.blockTopRightWidth;
		this.blockMiddleRightHeight=0.20f*(0.9f*this.heightScreen-5*this.gapBetweenblocks-this.blockBottomHeight);
		this.blockBottomRightWidth = this.blockTopRightWidth;
		this.blockBottomRightHeight = 0.65f*(0.9f*this.heightScreen-5*this.gapBetweenblocks-this.blockBottomHeight);
		
		this.blockTopLeft = new Rect (this.gapBetweenblocks, 0.1f * this.heightScreen + this.gapBetweenblocks, this.blockTopLeftWidth, this.blockTopLeftHeight);
		this.blockBottom = new Rect (this.gapBetweenblocks, 0.1f * this.heightScreen + 2 * this.gapBetweenblocks + this.blockTopLeftHeight, this.blockBottomWidth, this.blockBottomHeight);
		this.blockTopRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 0.1f * this.heightScreen + this.gapBetweenblocks, this.blockTopRightWidth, this.blockTopRightHeight);
		this.blockMiddleRight = new Rect (this.blockTopLeftWidth + 2 * this.gapBetweenblocks, 0.1f * this.heightScreen + 2 * this.gapBetweenblocks + this.blockTopRightHeight, this.blockMiddleRightWidth, this.blockMiddleRightHeight);
		this.blockBottomRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 0.1f * this.heightScreen + 3 * this.gapBetweenblocks + this.blockTopRightHeight+this.blockMiddleRightHeight, this.blockBottomRightWidth, this.blockBottomRightHeight);

		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		
		this.centralWindowStyle.fixedWidth = this.widthScreen*0.5f-5;
		
		this.centralWindowTitleStyle.fontSize = this.heightScreen*2/100;
		this.centralWindowTitleStyle.fixedHeight = (int)this.heightScreen*3/100;
		this.centralWindowTitleStyle.fixedWidth = (int)this.widthScreen*5/10;
		
		this.centralWindowButtonStyle.fontSize = this.heightScreen*2/100;
		this.centralWindowButtonStyle.fixedHeight = (int)this.heightScreen*3/100;
		this.centralWindowButtonStyle.fixedWidth = (int)this.widthScreen*20/100;
	}
}
