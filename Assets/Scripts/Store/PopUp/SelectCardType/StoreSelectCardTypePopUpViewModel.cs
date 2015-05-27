using UnityEngine;

public class StoreSelectCardTypePopUpViewModel
{	

	public bool guiEnabled;
	public string[] cardTypes; 
	public int cardTypeSelected;
	public string error;

	public StoreSelectCardTypePopUpViewModel ()
	{
		this.guiEnabled = false;
		this.cardTypes=new string[0];
		this.cardTypeSelected = -1;
		this.error = "";
	}
}


