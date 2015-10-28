using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewHomePageTabController : MonoBehaviour 
{	
	
	public int id;
	private bool isActive;

	void OnMouseOver()
	{
		if(!isActive)
		{
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
		}
	}
	void OnMouseExit()
	{
		if(!isActive)
		{
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
		}
	}
	public void setActive(bool value)
	{
		this.isActive = value;
	}
	public void OnMouseDown()
	{
		NewHomePageController.instance.selectATabHandler(this.id);	
	}
}

