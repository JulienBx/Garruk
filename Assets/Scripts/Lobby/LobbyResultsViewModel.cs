using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyResultsViewModel
{
	public GUIStyle[] styles;
	public IList<GUIStyle> resultsStyles;
	public IList<GUIStyle> resultsGameTypeStyles;
	public IList<GUIStyle> focusButtonStyles;
	public IList<string> resultsLabel;
	public GUIStyle wonResultStyle;
	public GUIStyle lostResultStyle;
	public GUIStyle friendlyGameStyle;
	public GUIStyle cupGameStyle;
	public GUIStyle divisionGameStyle;
	public GUIStyle focusButtonStyle;
	public GUIStyle selectedFocusButtonStyle;
	public GUIStyle resultsTitleStyle;
	public int chosenResult;
	public string resultsTitle;
	public Vector2 scrollPosition;


	public LobbyResultsViewModel ()
	{
		this.chosenResult = 0;
		this.styles=new GUIStyle[0];
		this.scrollPosition= new Vector2(0, 0);
		this.focusButtonStyles = new List<GUIStyle> ();
		this.resultsStyles = new List<GUIStyle> ();
		this.resultsGameTypeStyles = new List<GUIStyle> ();
		this.resultsLabel = new List<string> ();
		this.wonResultStyle = new GUIStyle ();
		this.lostResultStyle = new GUIStyle ();
		this.friendlyGameStyle = new GUIStyle ();
		this.cupGameStyle = new GUIStyle ();
		this.divisionGameStyle = new GUIStyle ();
		this.selectedFocusButtonStyle = new GUIStyle ();
		this.focusButtonStyle = new GUIStyle ();
		this.resultsTitleStyle = new GUIStyle ();
		this.resultsTitle = "Historiques de vos dernières rencontres";
	}
	public void initStyles()
	{
		this.wonResultStyle = this.styles [0];
		this.lostResultStyle = this.styles [1];
		this.friendlyGameStyle = this.styles [2];
		this.cupGameStyle = this.styles [3];
		this.divisionGameStyle = this.styles [4];
		this.focusButtonStyle = this.styles [5];
		this.selectedFocusButtonStyle = this.styles [6];
		this.resultsTitleStyle = this.styles [7];
	}
	public void resize(int heightScreen)
	{
		for (int i=0;i<this.resultsStyles.Count;i++)
		{
			this.resultsStyles[i].fontSize=heightScreen*2/100;
		}
		for (int i=0;i<this.focusButtonStyles.Count;i++)
		{
			this.focusButtonStyles[i].fontSize=heightScreen*2/100;
		}
	}
}

