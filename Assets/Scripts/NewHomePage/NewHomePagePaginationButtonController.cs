using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewHomePagePaginationButtonController : MonoBehaviour 
{	
	
	public int id;
	private bool isHovered;
	
	void OnMouseOver()
	{
		if(!this.isHovered)
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.isHovered=true;
		}
	}
	void OnMouseExit()
	{
		if(this.isHovered)
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.isHovered=false;
		}
	}
	public void setIsHoverd(bool value)
	{
		this.isHovered = value;
		if(value)
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
		}
		else
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
		}
	}
	public void OnMouseDown()
	{
		NewHomePageController.instance.paginationHandler(this.id);
	}
}

