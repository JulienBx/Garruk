using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class SkillBookSkillsViewModel
{
	public GUIStyle[] styles;
	public IList<int> skillsToBeDisplayed;
	public IList<string> names;
	public IList<string> descriptions;
	public IList<int> nbCards;
	public IList<int> percentages;
	public IList<GUIStyle> pictures;
	public IList<GUIStyle> gauges;
	public IList<float> gaugesWidth;
	public IList<float> gaugesBackgroundWidth;
	public GUIStyle nameStyle;
	public GUIStyle descriptionStyle;
	public GUIStyle percentageStyle;
	public GUIStyle nbCardsStyle;
	public GUIStyle buttonStyle;
	public GUIStyle gaugeBackgroundStyle;
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;
	public float gapBetweenBlocks;
	public int nbPages;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	
	public SkillBookSkillsViewModel ()
	{
		this.blocks=new Rect[0];
		this.skillsToBeDisplayed = new List<int> ();
		this.nameStyle = new GUIStyle ();
		this.descriptionStyle = new GUIStyle ();
		this.percentageStyle = new GUIStyle ();
		this.nbCardsStyle = new GUIStyle ();
		this.gaugeBackgroundStyle = new GUIStyle ();
		this.names = new List<string> ();
		this.descriptions = new List<string> ();
		this.nbCards = new List<int> ();
		this.percentages = new List<int> ();
		this.pictures = new List<GUIStyle> ();
		this.gauges = new List<GUIStyle> ();
		this.buttonStyle = new  GUIStyle ();
	}
	public void initStyles()
	{
		this.nameStyle = this.styles [0];
		this.descriptionStyle = this.styles [1];
		this.percentageStyle = this.styles [2];
		this.nbCardsStyle = this.styles [3];
		this.buttonStyle = this.styles [4];
		this.gaugeBackgroundStyle = this.styles [5];
	}
	public void resize(int heightScreen)
	{
		this.nameStyle.fontSize = heightScreen * 2 / 100;
		this.descriptionStyle.fontSize = heightScreen * 19 / 1000;
		this.percentageStyle.fontSize = heightScreen * 15 / 1000;
		this.nbCardsStyle.fontSize = heightScreen * 2 / 100;
		this.buttonStyle.fontSize = heightScreen * 17 / 1000;
	}
}

