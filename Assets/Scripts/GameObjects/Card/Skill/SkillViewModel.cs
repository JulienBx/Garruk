using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillViewModel
{
	public Texture[] skillLevel;
	public int number;
	public string name;
	public string description;
	public int power;
	public int manaCost;
	public int guiDepth;
	public Texture picto;
	public Rect popUpPosition;
	public GUIStyle[] styles;
	public GUIStyle centralWindowStyle;
	public GUIStyle descriptionStyle;
	public GUIStyle titleStyle;

	public SkillViewModel ()
	{
		this.skillLevel=new Texture[6];
		this.popUpPosition = new Rect ();
		this.styles=new GUIStyle[0];
		this.centralWindowStyle = new GUIStyle ();
		this.descriptionStyle = new GUIStyle ();
	}
	public void initStyles()
	{
		this.centralWindowStyle = this.styles [0];
		this.descriptionStyle = this.styles [1];
		this.titleStyle = this.styles [2];
	}
	public void resize()
	{
		this.descriptionStyle.fontSize =Screen.height * 2 / 100;
	}
}
