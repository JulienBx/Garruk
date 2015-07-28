using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class CompetitionController : MonoBehaviour 
{	

	private int id;

	void Awake()
	{

	}
	void OnMouseOver()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Border").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public void OnMouseDown()
	{

	}
	public void show(string title)
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text=title;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = null;
	}
	public void setPicture(Sprite picture)
	{
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = picture;
	}
}

