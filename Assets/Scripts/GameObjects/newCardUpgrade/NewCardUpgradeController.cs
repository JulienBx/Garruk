using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewCardUpgradeController : MonoBehaviour 
{
	
	public void setCardUpgrade(int increase)
	{
		this.gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "+ " + increase.ToString ();
	}
}

