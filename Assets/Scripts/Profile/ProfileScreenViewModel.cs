using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfileScreenViewModel {
	
	public int heightScreen=Screen.height;
	public int widthScreen=Screen.width;

	public bool displayTopRightBlock=false;

	public float blockLeftWidth;
	public float blockLeftHeight;
	public float blockBottomCenterRightWidth;
	public float blockBottomCenterRightHeight;
	public float blockBottomCenterLeftWidth;
	public float blockBottomCenterLeftHeight;
	public float blockTopCenterWidth;
	public float blockTopCenterHeight;
	public float blockTopRightWidth;
	public float blockTopRightHeight;
	public float blockMiddleRightWidth;
	public float blockMiddleRightHeight;
	public float blockBottomRightWidth;
	public float blockBottomRightHeight;
	public float gapBetweenblocks;
	
	public Rect blockLeft;
	public Rect blockBottomCenterRight;
	public Rect blockBottomCenterLeft;
	public Rect blockTopCenter;
	public Rect blockTopRight;
	public Rect blockMiddleRight;
	public Rect blockBottomRight;

	public Rect centralWindow;
	public Rect fileBrowserWindow;

	public GUIStyle[] styles;

	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	public GUIStyle centralWindowButtonStyle;
	public GUIStyle blockBorderStyle;
	public GUIStyle centralWindowTextfieldStyle;

	public Texture2D m_directoryImage;
	public Texture2D m_fileImage;

	public GUISkin fileBrowserSkin;
	
	public ProfileScreenViewModel (){
	}
	public ProfileScreenViewModel (Texture2D m_directoryimage, Texture2D m_fileimage, GUISkin filebrowserskin){
		this.m_directoryImage = m_directoryimage;
		this.m_fileImage = m_fileimage;
		this.fileBrowserSkin = filebrowserskin;
	}
	public void initStyles(){
		this.centralWindowStyle = this.styles [0];
		this.centralWindowTitleStyle = this.styles [1];
		this.centralWindowButtonStyle = this.styles [2];
		this.blockBorderStyle = this.styles [3];
		this.centralWindowTextfieldStyle=this.styles[4];
	}
	public void resize(){
		this.heightScreen = Screen.height;
		this.widthScreen = Screen.width;

		this.gapBetweenblocks = 5;

		this.blockLeftHeight=1f*(0.9f*this.heightScreen-2*this.gapBetweenblocks);

		if(this.heightScreen/3>180)
		{
			this.blockLeftWidth = 180;
		}
		else
		{
			this.blockLeftWidth = this.heightScreen/3;
		}

		this.blockTopRightHeight=System.Convert.ToInt32(this.displayTopRightBlock)*0.10f*(0.9f*this.heightScreen-4*this.gapBetweenblocks);
		this.blockTopRightWidth = System.Convert.ToInt32 (this.displayTopRightBlock) * 180;
		
		this.blockMiddleRightHeight=0.45f*(0.9f*this.heightScreen-4*this.gapBetweenblocks);
		this.blockMiddleRightWidth = this.blockLeftWidth ;
		
		this.blockBottomRightHeight=0.45f*(0.9f*this.heightScreen-4*this.gapBetweenblocks);
		this.blockBottomRightWidth = this.blockLeftWidth ;

		this.blockTopCenterHeight = 0.5f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockTopCenterWidth =  this.widthScreen - (4 * this.gapBetweenblocks)-this.blockLeftWidth-this.blockMiddleRightWidth;

		this.blockBottomCenterLeftHeight= 0.5f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockBottomCenterLeftWidth=  0.5f * (this.widthScreen - (5 * this.gapBetweenblocks)-this.blockLeftWidth-this.blockMiddleRightWidth);

		this.blockBottomCenterRightHeight= 0.5f * (0.9f * this.heightScreen - 3 * this.gapBetweenblocks);
		this.blockBottomCenterRightWidth=  0.5f * (this.widthScreen - (5 * this.gapBetweenblocks)-this.blockLeftWidth-this.blockMiddleRightWidth);


		
		this.blockLeft = new Rect (this.gapBetweenblocks, 
		                           0.1f * this.heightScreen + this.gapBetweenblocks, 
		                           this.blockLeftWidth, 
		                           this.blockLeftHeight);

		this.blockTopCenter = new Rect (this.blockLeftWidth+2*this.gapBetweenblocks, 
		                                0.1f * this.heightScreen + this.gapBetweenblocks,
		                                this.blockTopCenterWidth, 
		                                this.blockTopCenterHeight);

		this.blockBottomCenterLeft = new Rect (this.blockLeftWidth+2*this.gapBetweenblocks,
		                                       0.1f * this.heightScreen + 2*this.gapBetweenblocks + this.blockTopCenterHeight,
		                                       this.blockBottomCenterLeftWidth, this.blockBottomCenterLeftHeight);

		this.blockBottomCenterRight = new Rect (this.blockLeftWidth+3*this.gapBetweenblocks+this.blockBottomCenterLeftWidth,
		                                       0.1f * this.heightScreen + 2*this.gapBetweenblocks + this.blockTopCenterHeight,
		                                       this.blockBottomCenterRightWidth, this.blockBottomCenterRightHeight);

		this.blockTopRight = new Rect (this.blockLeftWidth + 3 * this.gapBetweenblocks + this.blockTopCenterWidth,
		                               0.1f * this.heightScreen + this.gapBetweenblocks,
		                               this.blockTopRightWidth, 
		                               this.blockTopRightHeight);

		this.blockMiddleRight = new Rect (this.blockLeftWidth+3*this.gapBetweenblocks+this.blockTopCenterWidth,
		                                  0.1f * this.heightScreen + this.gapBetweenblocks+System.Convert.ToInt32(this.displayTopRightBlock)*(this.gapBetweenblocks+this.blockTopRightHeight),
		                                  this.blockMiddleRightWidth,
		                                  this.blockMiddleRightHeight);

		this.blockBottomRight = new Rect (this.blockLeftWidth+3*this.gapBetweenblocks+this.blockTopCenterWidth,
		                                  0.1f * this.heightScreen + 2*this.gapBetweenblocks+blockMiddleRightHeight+System.Convert.ToInt32(this.displayTopRightBlock)*(this.gapBetweenblocks+this.blockTopRightHeight),
		                                  this.blockBottomRightWidth,
		                                  this.blockBottomRightHeight);

		this.centralWindow = new Rect (this.widthScreen * 0.25f, 0.12f * this.heightScreen, this.widthScreen * 0.50f, 0.25f * this.heightScreen);
		this.fileBrowserWindow = new Rect (this.widthScreen * 0.25f, 0.125f * this.heightScreen, this.widthScreen * 0.50f, 0.75f * this.heightScreen);

		this.centralWindowStyle.fixedWidth = this.widthScreen*0.5f-5;
		this.centralWindowTitleStyle.fontSize = this.heightScreen*2/100;
		this.centralWindowButtonStyle.fontSize = this.heightScreen*2/100;
		this.centralWindowTextfieldStyle.fontSize = this.heightScreen*2/100;
	}
}
