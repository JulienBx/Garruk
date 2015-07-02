using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuViewModel {

	public bool[] buttonsEnabled;
	public bool displayAdmin;
	public GUIStyle buttonStyle;
	public GUIStyle logoStyle;
	public string[] buttonsLabels;
	
	public newMenuViewModel ()
	{
		this.displayAdmin = false;
		this.buttonStyle = new GUIStyle ();
		this.buttonsEnabled = new bool[7];
		this.buttonsLabels = new string[5];
		this.buttonsLabels [0] = "Accueil";
		this.buttonsLabels [1] = "Mes cartes";
		this.buttonsLabels [2] = "Boutique";
		this.buttonsLabels [3] = "Bazar";
		this.buttonsLabels [4] = "Jouer";
		for(int i=0;i<this.buttonsEnabled.Length;i++)
		{
			this.buttonsEnabled[i]=true;
		}
	}
	public void resize(float buttonsHeight, float buttonsWidth)
	{	
		if(buttonsWidth<(3f*buttonsHeight))
		{
			this.buttonStyle.fontSize = (int)buttonsHeight * 25/100;
		}
		else
		{
			this.buttonStyle.fontSize = (int)buttonsHeight * 3/10;
		}
		this.buttonStyle.padding.top = (int)buttonsHeight * 1/10;
		this.buttonStyle.padding.bottom = (int)buttonsHeight * 15/100;
		this.buttonStyle.padding.right=(int)buttonsWidth * 7/100;
	}
	
}

