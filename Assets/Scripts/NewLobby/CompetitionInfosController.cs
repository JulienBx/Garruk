using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class CompetitionInfosController : MonoBehaviour 
{	

	Competition c;

	void Awake()
	{
		
	}
	void OnMouseOver()
	{
		//gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		//gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		//gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);

	}
	void OnMouseExit()
	{
		//gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		//gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		//gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	public void OnMouseDown()
	{
		//NewHomePageController.instance.joinGame(this.id);	
	}
	public void show(Competition c)
	{
		//gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=title;
	}
	public void setPicture(Sprite picture)
	{
		//gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = picture;
	}
}

