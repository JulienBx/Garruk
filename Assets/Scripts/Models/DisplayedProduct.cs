﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayedProduct 
{
	public int Id;
	public float PriceEUR;
	public float PriceUSD;
	public float PriceGBP;
	public float Crystals;
	public string ProductID;
	public string ProductNameApple;
	public string ProductNameGooglePlay;
	
	public DisplayedProduct()
	{
		this.ProductID="";
		this.ProductNameApple="";
		this.ProductNameGooglePlay="";
	}
}


