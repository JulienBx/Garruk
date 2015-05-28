using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageCompetsViewModel 
{
	public GUIStyle[] styles;
	public IList<string> competsNames;
	public IList<GUIStyle> competsButtonsStyle;
	public GUIStyle nameStyle;
	public GUIStyle buttonStyle;
	public GUIStyle labelNoStyle;
	public string labelNo;
	
	public HomePageCompetsViewModel ()
	{
		this.buttonStyle = new GUIStyle ();
		this.nameStyle = new GUIStyle ();
		this.competsButtonsStyle = new List<GUIStyle> ();
		this.competsNames = new List<string> ();
		this.labelNoStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.nameStyle = this.styles [0];
		this.buttonStyle = this.styles [1];
		this.labelNoStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
		this.nameStyle.fontSize = heightScreen*2/100;
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
		this.labelNoStyle.fontSize = heightScreen * 2 / 100;
	}
}