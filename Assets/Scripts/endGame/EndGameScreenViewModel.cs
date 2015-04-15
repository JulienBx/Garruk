using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScreenViewModel {

	public int heightScreen=Screen.height;
	public int widthScreen=Screen.width;

	public float blockTopLeftWidth;
	public float blockTopLeftHeight;
	public float blockTopRightWidth;
	public float blockTopRightHeight;
	public float blockBottomLeftWidth;
	public float blockBottomLeftHeight;
	public float blockBottomRightWidth;
	public float blockBottomRightHeight;
	public float gapBetweenblocks;

	public Rect blockTopLeft;
	public Rect blockTopRight;
	public Rect blockBottomLeft;
	public Rect blockBottomRight;

	public Rect centralWindow;

	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle blockBorderStyle;
	
	public EndGameScreenViewModel (){
	}

	public void computeScreenDisplay(){
		this.gapBetweenblocks = 0.005f * this.widthScreen;
		this.blockTopLeftHeight = 0.66f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		this.blockTopLeftWidth = 0.75f*(this.widthScreen-3*this.gapBetweenblocks);
		this.blockBottomLeftHeight = 0.34f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		this.blockBottomLeftWidth = this.blockTopLeftWidth;
		this.blockTopRightWidth = 0.25f*(this.widthScreen-3*this.gapBetweenblocks);
		this.blockTopRightHeight = 0.30f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		this.blockBottomRightWidth = this.blockTopRightWidth;
		this.blockBottomRightHeight = 0.70f*(0.9f*this.heightScreen-3*this.gapBetweenblocks);
		
		this.blockTopLeft = new Rect (this.gapBetweenblocks, 0.1f * this.heightScreen + this.gapBetweenblocks, this.blockTopLeftWidth, this.blockTopLeftHeight);
		this.blockBottomLeft = new Rect (this.gapBetweenblocks, 0.1f * this.heightScreen + 2 * this.gapBetweenblocks + this.blockTopLeftHeight, this.blockBottomLeftWidth, this.blockBottomLeftHeight);
		this.blockTopRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 0.1f * this.heightScreen + this.gapBetweenblocks, this.blockTopRightWidth, this.blockTopRightHeight);
		this.blockBottomRight = new Rect (this.blockTopLeftWidth+2*this.gapBetweenblocks, 0.1f * this.heightScreen + 2 * this.gapBetweenblocks + this.blockTopRightHeight, this.blockBottomRightWidth, this.blockBottomRightHeight);

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
