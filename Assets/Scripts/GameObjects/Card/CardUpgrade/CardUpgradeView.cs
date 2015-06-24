using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardUpgradeView : MonoBehaviour
{	
	public CardUpgradeViewModel VM;
	private int skillId;
	private bool isHovered=false;

	public CardUpgradeView ()
	{
		this.VM = new CardUpgradeViewModel ();
	}

	void OnGUI () 
	{
		GUI.depth = -3/4;
		GUILayout.BeginArea (VM.cardUpgradeRect);
		{
			GUILayout.Label(VM.value,VM.upgradeStyle);
		}
		GUILayout.EndArea ();
	}	
}

