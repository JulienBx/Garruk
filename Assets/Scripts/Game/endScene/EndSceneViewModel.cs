using System;
using UnityEngine;
public class EndSceneViewModel
{
	public int credits;
	public int startCredits;
	public int endCredits;
	public int creditsToAdd;
	public GUIStyle[] styles;
	public GUIStyle titleStyle;
	public GUIStyle creditStyle;
	public GUIStyle labelStyle;
	public GUIStyle buttonStyle;

	public EndSceneViewModel ()
	{
		this.titleStyle = new GUIStyle ();
		this.creditStyle = new GUIStyle ();
		this.labelStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.titleStyle = this.styles [0];
		this.creditStyle = this.styles [1];
		this.labelStyle = this.styles [2];
		this.buttonStyle = this.styles [3];
	}
	public void resize(int heightScreen)
	{
		this.titleStyle.fontSize = heightScreen * 3 / 100;
		this.creditStyle.fontSize = heightScreen * 2 / 100;
		this.labelStyle.fontSize = heightScreen * 2 / 100;
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
	}
}