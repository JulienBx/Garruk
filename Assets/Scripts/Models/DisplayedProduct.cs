using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
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
	public bool IsActive;
	
	public DisplayedProduct()
	{
		this.ProductID="";
		this.ProductNameApple="";
		this.ProductNameGooglePlay="";
	}
}



