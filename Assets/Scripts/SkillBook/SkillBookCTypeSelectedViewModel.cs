using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class SkillBookCTypeSelectedViewModel
{
	public GUIStyle[] styles;
	public GUIStyle nameStyle;
	public GUIStyle pictureStyle;
	public GUIStyle percentageStyle;
	public GUIStyle nbCardsStyle;
	public GUIStyle buttonStyle;
	public GUIStyle gaugeStyle;
	public GUIStyle gaugeBackgroundStyle;
	public int ctypeSelected;
	public int nbCards;
	public string name;
	public string percentage;
	public float gaugeWidth;
	public float gaugeBackgroundWidth;
	
	public SkillBookCTypeSelectedViewModel ()
	{
		this.ctypeSelected = 0;
		this.name = "";
		this.percentage = "";
		this.nameStyle = new GUIStyle ();
		this.pictureStyle = new GUIStyle ();
		this.percentageStyle = new GUIStyle ();
		this.nbCardsStyle = new GUIStyle ();
		this.buttonStyle = new GUIStyle ();
		this.gaugeStyle = new GUIStyle ();
		this.gaugeBackgroundStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.nameStyle = this.styles [0];
		this.percentageStyle = this.styles [1];
		this.nbCardsStyle = this.styles [2];
		this.buttonStyle = this.styles [3];
		this.gaugeStyle = this.styles [4];
		this.gaugeBackgroundStyle = this.styles [5];
	}
	public void resize(int heightScreen)
	{
		this.nameStyle.fontSize = heightScreen * 25 / 1000;
		this.percentageStyle.fontSize = heightScreen * 15 / 1000;
		this.nbCardsStyle.fontSize = heightScreen * 25 / 1000;
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
	}
}

