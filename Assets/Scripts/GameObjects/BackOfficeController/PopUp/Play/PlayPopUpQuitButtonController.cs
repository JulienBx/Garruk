using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PlayPopUpQuitButtonController : MonoBehaviour 
{	
	
	void Awake()
	{
		
	}
	void OnMouseOver()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}
	}
	void OnMouseExit()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	public void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<PlayPopUpController> ().quitPopUp ();
	}
}

