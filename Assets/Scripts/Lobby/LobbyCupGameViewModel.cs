using System;
using UnityEngine;
public class LobbyCupGameViewModel
{
	public GUIStyle[] styles;
	public GUIStyle buttonStyle;
	public GUIStyle labelStyle;
	public string cupInformationsLabel;
	
	public LobbyCupGameViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.buttonStyle = new GUIStyle ();
		this.labelStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.buttonStyle = this.styles [0];
		this.labelStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.buttonStyle.fontSize= heightScreen * 2 / 100;
		this.labelStyle.fontSize= heightScreen * 3 / 100;
	}
}

