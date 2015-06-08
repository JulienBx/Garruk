using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class StoreNewSkillsPopUpViewModel
{	
	
	public IList<string> skills;
	public string title;
	public int guiDepth;
	public Rect centralWindow;
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle centralWindowTitleStyle;
	
	public StoreNewSkillsPopUpViewModel ()
	{
		this.skills = new List<string> ();
		this.title = "";
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


