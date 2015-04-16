using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyTrophiesViewModel {
	
	public IList<GUIStyle> trophiesPicturesButtonStyle;
	public IList<DisplayedTrophy> trophies;
	public int nbPages;
	public int pageDebut;
	public int pageFin;
	public int chosenPage;
	public int start;
	public int finish;
	public int elementPerRow;
	public GUIStyle[] paginatorGuiStyle;
	public GUIStyle[] styles;
	public string labelNo;
	public string title="Mes trophées";
	public Rect[] blocks;
	public float blocksWidth;
	public float blocksHeight;
	public GUIStyle trophyNameStyle;
	public GUIStyle trophyDateStyle;

	
	public MyTrophiesViewModel ()
	{
	}
	public void initStyles(){
		this.trophyNameStyle = this.styles [0];
		this.trophyDateStyle = this.styles [1];
	}
	public void resize(int heightScreen)
	{
		this.trophyNameStyle.fontSize = heightScreen*25/1000;
		this.trophyDateStyle.fontSize = heightScreen*25/1000;
	}
	public void displayPage()
	{
		this.start = this.chosenPage*(this.elementPerRow*3);
		if (this.trophies.Count < (3*this.elementPerRow*(this.chosenPage+1)))
		{
			this.finish = this.trophies.Count;
		}
		else{
			this.finish = (this.chosenPage+1)*(3 * this.elementPerRow);
		}
	}
}
