using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardUpgradeController : GameObjectController {
	
	public GUIStyle[] styles;
	private CardUpgradeView view;
	
	void Awake () 
	{	
		this.view = gameObject.AddComponent <CardUpgradeView>();
	}
	public void resize(Rect cardUpgradeRect)
	{
		view.VM.resize (cardUpgradeRect);
	}
	public void setCardUpgrade(Rect cardUpgradeRect, int increase)
	{
		this.initStyles ();
		this.resize (cardUpgradeRect);
		view.VM.value = "+ " + increase.ToString ();
	}
	public void initStyles()
	{
		view.VM.styles=new GUIStyle[this.styles.Length];
		for(int i=0;i<this.styles.Length;i++)
		{
			view.VM.styles[i]=this.styles[i];
		}
		view.VM.initStyles();
	}
}

