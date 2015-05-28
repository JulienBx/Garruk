using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePagePacksViewModel 
{
	
	public GUIStyle[] styles;
	public IList<GUIStyle> packPicturesButtonStyle;
	public GUIStyle buttonStyle;
	public GUIStyle newPackStyle;
	public GUIStyle packNameStyle;
	public GUIStyle labelNoStyle;
	public IList<string> packsNames;
	public IList<bool> packsNew;
	public string labelNo;
	public int nbPages;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;
	
	public HomePagePacksViewModel ()
	{
		this.buttonStyle = new GUIStyle ();
		this.newPackStyle = new GUIStyle ();
		this.packNameStyle = new GUIStyle ();
		this.labelNoStyle = new GUIStyle ();
		this.blocks=new Rect[0];
	}
	public void initStyles()
	{	
		this.buttonStyle = this.styles [0];
		this.newPackStyle = this.styles [1];
		this.packNameStyle = this.styles [2];
		this.labelNoStyle = this.styles [3];

	}
	public void resize(int heightScreen)
	{	
		this.buttonStyle.fontSize = heightScreen * 2 / 100;
		this.newPackStyle.fontSize = heightScreen * 2 / 100;
		this.packNameStyle.fontSize = heightScreen * 2 / 100;
		this.labelNoStyle.fontSize = heightScreen * 2 / 100;
	}
}