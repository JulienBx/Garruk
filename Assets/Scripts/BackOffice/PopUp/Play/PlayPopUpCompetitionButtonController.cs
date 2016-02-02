using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PlayPopUpCompetitionButtonController : MonoBehaviour 
{	
	
	private int id;
	
	void Awake()
	{
		
	}
	void OnMouseOver()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		}

	}
	void OnMouseExit()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public void OnMouseDown()
	{
		gameObject.transform.parent.GetComponent<PlayPopUpController> ().selectGame (this.id);	
	}
}

