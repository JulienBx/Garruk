using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewHomePageDeckSelectionButtonController : MonoBehaviour 
{	

	private bool isHovered;
	
	void OnMouseOver()
	{
		if(!isHovered)
		{
			this.gameObject.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.isHovered=true;
			NewHomePageController.instance.mouseOnSelectDeckButton(true);
		}
	}
	void OnMouseExit()
	{
		if(isHovered)
		{
			this.gameObject.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.isHovered=false;
			NewHomePageController.instance.mouseOnSelectDeckButton(false);
		}
	}
	public void OnMouseDown()
	{
		NewHomePageController.instance.deckSelectionButtonHandler();	
	}
}

