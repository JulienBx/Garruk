using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LoadingScreenButtonController : MonoBehaviour 
{
	private bool isHovering;
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		MenuController.instance.leaveRandomRoomHandler ();
	}
	public bool getIsHovering()
	{
		return isHovering;
	}
}

