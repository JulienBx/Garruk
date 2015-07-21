using UnityEngine;

public class NewPackSelectCardTypePopUpViewModel
{	
	
	public bool guiEnabled;
	public string[] cardTypes; 
	public int cardTypeSelected;
	public string error;
	
	public NewPackSelectCardTypePopUpViewModel ()
	{
		this.guiEnabled = true;
		this.cardTypes=new string[0];
		this.cardTypeSelected = -1;
		this.error = "";
	}
}


