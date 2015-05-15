using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameViewModel {
	
	public int gameType;
	
	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	
	public EndGameViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.buttonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
	}
	public void resize(int heightScreen)
	{
		this.buttonStyle.fontSize = heightScreen * 3 / 100;
	}
}
