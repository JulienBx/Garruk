using UnityEngine;

public class NewStoreSelectCardTypePopUpViewModel
{	
	
	public bool guiEnabled;
	public string[] cardTypes; 
	public int cardTypeSelected;
	public string error;
	
	public NewStoreSelectCardTypePopUpViewModel ()
	{
		this.guiEnabled = true;
		this.cardTypes=new string[0];
		this.cardTypeSelected = -1;
		this.error = "";
	}
}


