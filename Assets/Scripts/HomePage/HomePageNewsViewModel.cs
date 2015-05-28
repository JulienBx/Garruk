using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageNewsViewModel {
	
	public GUIStyle[] styles;
	public IList<GUIStyle> profilePicturesButtonStyle;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public string labelNo;
	
	public GUIStyle[] paginatorGuiStyle;
	public GUIStyle newsContentStyle;
	public GUIStyle newsDateStyle;
	
	public IList<string> username;
	public IList<int> totalNbWins;
	public IList<int> totalNbLooses;
	public IList<int> ranking;
	public IList<int> division;
	public IList<string> content;
	public IList<DateTime> date;
	
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;
	
	public HomePageNewsViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.profilePicturesButtonStyle = new List<GUIStyle> ();
		this.paginatorGuiStyle=new GUIStyle[0];
		this.newsContentStyle = new GUIStyle ();
		this.newsDateStyle = new GUIStyle ();
		this.blocks=new Rect[0];
	}
	public void initStyles()
	{	
		this.newsContentStyle = this.styles [0];
		this.newsDateStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{	
		this.newsContentStyle.fontSize = heightScreen * 2 / 100;
		this.newsDateStyle.fontSize = heightScreen * 2 / 100;
	}
}