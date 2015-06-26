using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardUpgradeViewModel
{
	public GUIStyle[] styles;
	public GUIStyle upgradeStyle;
	public Rect cardUpgradeRect;
	public string value;
	
	public CardUpgradeViewModel ()
	{
		this.styles=new GUIStyle[0];
		this.upgradeStyle = new GUIStyle ();
		this.cardUpgradeRect = new Rect ();
		this.value = "";
	}
	public void initStyles()
	{
		this.upgradeStyle = this.styles [0];
	}
	public void resize(Rect cardUpgradeRect)
	{
		this.cardUpgradeRect = cardUpgradeRect;
		this.upgradeStyle.fontSize = (int)this.cardUpgradeRect.height;
	}
}
