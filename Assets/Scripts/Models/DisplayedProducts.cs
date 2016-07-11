using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DisplayedProducts{

	public List<DisplayedProduct> products ;
	
	public DisplayedProducts()
	{
		this.products = new List<DisplayedProduct>();
	}
	public DisplayedProduct getProduct(int index)
	{
		return this.products [index];
	}
	public int getCount()
	{
		return this.products.Count;
	}
	public void add()
	{
		this.products.Add(new DisplayedProduct());
	}
	public void parseProducts(string s)
	{
		string[] array = s.Split (new string[]{"PRODUCT"},System.StringSplitOptions.None);
		for(int i=0;i<array.Length-1;i++)
		{
			string[] ProductInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			this.products.Add (new DisplayedProduct());
			products[i].Id = System.Convert.ToInt32(ProductInformation[0]);
			products[i].PriceEUR = float.Parse(ProductInformation[1]);
			products[i].Crystals=float.Parse(ProductInformation[2]);
			products[i].ProductID=ProductInformation[3];
			products[i].ProductNameApple=ProductInformation[4];
			products[i].ProductNameGooglePlay=ProductInformation[5];
			products[i].PriceUSD = float.Parse(ProductInformation[6]);
			products[i].PriceGBP = float.Parse(ProductInformation[7]);
			products[i].IsActive = System.Convert.ToBoolean(System.Convert.ToInt32(ProductInformation[8]));
		}
	}
}