using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class SkillBookBookViewModel
{
	public GUIStyle[] styles;
	public GUIStyle bookBackgroundStyle;
	public GUIStyle nextButtonStyle;
	public GUIStyle backButtonStyle;
	
	public SkillBookBookViewModel ()
	{
		this.bookBackgroundStyle = new GUIStyle ();
		this.nextButtonStyle = new GUIStyle ();
		this.backButtonStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.bookBackgroundStyle = this.styles [0];
		this.nextButtonStyle = this.styles [1];
		this.backButtonStyle = this.styles [2];
	}
	public void resize(int heightScreen)
	{
	}
}

