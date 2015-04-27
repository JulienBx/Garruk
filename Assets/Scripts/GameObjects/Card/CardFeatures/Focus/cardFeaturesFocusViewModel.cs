using UnityEngine;

public class CardFeaturesFocusViewModel
{
	
	public bool guiEnabled;
	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	public GUIStyle statsStyle;
	
	
	public CardFeaturesFocusViewModel ()
	{
		this.guiEnabled = true;
		this.styles=new GUIStyle[0];
		this.buttonStyle = new GUIStyle ();
		this.statsStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
		this.statsStyle = this.styles [1];
	}
	public void resize(float rectHeight)
	{
		this.buttonStyle.fontSize = (int)rectHeight * 1 / 5;
		this.statsStyle.fontSize= (int)rectHeight * 1 / 5;	
	}
}

