using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DivisionLobbyResultsViewModel
{
	
	public GUIStyle[] styles;
	public GUIStyle[] paginatorGuiStyle;
	public IList<GUIStyle> profilePictureButtonStyles;
	public IList<GUIStyle> labelStyles;
	public IList<GUIStyle> backgroundStyles;
	public GUIStyle wonLabelStyle;
	public GUIStyle defeatLabelStyle;
	public GUIStyle usernameLabelStyle;
	public GUIStyle informationsLabelStyle;
	public GUIStyle wonBackgroundStyle;
	public GUIStyle defeatBackgroundStyle;
	public GUIStyle titleStyle;
	public IList<string> label;
	public IList<int> totalNbWins;
	public IList<int> totalNbLooses;
	public IList<int> ranking;
	public IList<int> division;
	public IList<string> username;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public string resultsTitle;
	
	public DivisionLobbyResultsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.paginatorGuiStyle = new GUIStyle[0];
		this.profilePictureButtonStyles = new List<GUIStyle> ();
		this.labelStyles = new List<GUIStyle> ();
		this.backgroundStyles = new List<GUIStyle> ();
		this.wonLabelStyle = new GUIStyle ();
		this.defeatLabelStyle = new GUIStyle ();
		this.usernameLabelStyle = new GUIStyle ();
		this.informationsLabelStyle = new GUIStyle ();
		this.wonBackgroundStyle = new GUIStyle ();
		this.defeatBackgroundStyle = new GUIStyle ();
		this.titleStyle = new GUIStyle ();
		this.label = new List<string> ();
		this.totalNbWins = new List<int> ();
		this.totalNbLooses = new List<int> ();
		this.ranking = new List<int> ();
		this.division = new List<int> ();
		this.username = new List<string> ();
		this.resultsTitle = "Derniers résultats de division";
	}
	public void initStyles()
	{
		this.wonLabelStyle = this.styles [0];
		this.defeatLabelStyle = this.styles [1];
		this.usernameLabelStyle = this.styles [2];
		this.informationsLabelStyle = this.styles [3];
		this.wonBackgroundStyle = this.styles [4];
		this.defeatBackgroundStyle = this.styles [5];
		this.titleStyle = this.styles [6];
	}
	public void resize(int heightScreen)
	{
		this.wonLabelStyle.fontSize=heightScreen * 2 / 100;
		this.defeatLabelStyle.fontSize = heightScreen * 2 / 100;
		this.usernameLabelStyle.fontSize = heightScreen * 2 / 100;
		this.informationsLabelStyle.fontSize = heightScreen * 15 / 1000;
		this.titleStyle.fontSize = heightScreen * 2 / 100;
	}
}