using System;
using UnityEngine;
public class FocusStoreFeaturesViewModel
{

	public Rect[] cardFeaturesFocusRects;
	
	public string title;
	public int renameCost;
	public int cardCost;
	public int nextLevelCost;
	public int cardLevel;
	public bool isOnSale;
	public int price;


	public FocusStoreFeaturesViewModel ()
	{
		this.cardFeaturesFocusRects=new Rect[5];
	}



}

