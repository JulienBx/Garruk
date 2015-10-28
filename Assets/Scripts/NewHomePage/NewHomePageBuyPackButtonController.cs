using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewHomePageBuyPackButtonController : MonoBehaviour 
{	
	
	public int id;
	private bool isHovered;
	
	void OnMouseOver()
	{
		if(!this.isHovered)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
			gameObject.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.isHovered=true;
			NewHomePageController.instance.mouseOnBuyPackButton(true);
		}
	}
	void OnMouseExit()
	{
		if(this.isHovered)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
			gameObject.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.isHovered=false;
			NewHomePageController.instance.mouseOnBuyPackButton(false);
		}
	}
	public void OnMouseDown()
	{
		NewHomePageController.instance.buyPackHandler();	
	}
}

