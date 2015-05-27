using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class StorePacksViewModel
{
	public GUIStyle[] styles;
	public GUIStyle[] paginatorGuiStyle;
	public GUIStyle packBuyButtonStyle;
	public GUIStyle packNameStyle;
	public GUIStyle packPriceStyle;
	public GUIStyle packNewStyle;
	public IList<string> packNames;
	public IList<int> packPrices;
	public IList<bool> guiEnabled;
	public IList<GUIStyle> packPictureStyles;
	public IList<bool> isNew;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int nbElementsToDisplay;

	public StorePacksViewModel ()
	{
		this.nbElementsToDisplay = 4;
		this.packBuyButtonStyle = new GUIStyle ();
		this.packPriceStyle = new GUIStyle ();
		this.packNewStyle = new GUIStyle ();
		this.packNameStyle = new GUIStyle ();
		this.start = 0;
		this.finish = 0;
	}
	public void initStyles()
	{
		this.packBuyButtonStyle = this.styles [0];
		this.packPriceStyle = this.styles [1];
		this.packNewStyle = this.styles [2];
		this.packNameStyle = this.styles [3];
	}
	public void resize(int heightScreen)
	{
		this.packNameStyle.fontSize = heightScreen * 3 / 100;
		this.packPriceStyle.fontSize = heightScreen * 2 / 100;
		this.packNewStyle.fontSize = heightScreen * 2 / 100;
		this.packBuyButtonStyle.fontSize = heightScreen * 2 / 100;
	}
}

