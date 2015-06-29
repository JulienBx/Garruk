using UnityEngine;

public class StoreCollectionPointsPopUpViewModel
{	

	public int collectionPoints;
	public int collectionPointsRanking;
	public int guiDepth;
	public Rect centralWindow;
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	
	public StoreCollectionPointsPopUpViewModel ()
	{
		this.guiDepth = -1;
		this.centralWindow = new Rect ();
		this.styles=new GUIStyle[0];
		this.centralWindowStyle = new GUIStyle ();
		this.centralWindowTitleStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.centralWindowStyle = this.styles [0];
		this.centralWindowTitleStyle = this.styles [1];
	}
	public void resize()
	{
		this.centralWindowTitleStyle.fontSize = Screen.height * 2 / 100;
	}
}


