using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class EndGameResultsViewModel
{
	
	public GUIStyle[] styles;
	public IList<GUIStyle> resultsStyles;
	public IList<GUIStyle> focusButtonStyles;
	public GUIStyle wonLabelStyle;
	public GUIStyle defeatLabelStyle;
	public GUIStyle selectedFocusButtonStyle;
	public GUIStyle focusButtonStyle;
	public GUIStyle titleStyle;
	public IList<string> resultsLabel;
	public int chosenResult;
	public string resultsTitle;
	public Vector2 scrollPosition;
	
	public EndGameResultsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.resultsStyles = new List<GUIStyle> ();
		this.focusButtonStyles = new List<GUIStyle> ();
		this.resultsLabel = new List<string> ();
		this.wonLabelStyle = new GUIStyle ();
		this.defeatLabelStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.focusButtonStyle = new GUIStyle ();
		this.selectedFocusButtonStyle = new GUIStyle ();
		this.resultsTitle = "Derniers résultats de division";
		this.scrollPosition= new Vector2(0, 0);
	}
	public void initStyles()
	{
		this.wonLabelStyle = this.styles [0];
		this.defeatLabelStyle = this.styles [1];
		this.titleStyle = this.styles [2];
		this.focusButtonStyle = this.styles [3];
		this.selectedFocusButtonStyle = this.styles [4];
	}
	public void resize(int heightScreen)
	{
		this.wonLabelStyle.fontSize=heightScreen * 2 / 100;
		this.defeatLabelStyle.fontSize = heightScreen * 2 / 100;
		this.titleStyle.fontSize = heightScreen * 2 / 100;
	}
}