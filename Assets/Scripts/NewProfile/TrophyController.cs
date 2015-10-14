using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class TrophyController : MonoBehaviour 
{
	
	public Trophy t;
	private int Id;
	private bool isHovering;
	
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
		}
	}
	void OnMouseDown()
	{
		
	}
	public void show()
	{
		gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.t.competition.Name;
		gameObject.transform.FindChild ("Date").GetComponent<TextMeshPro> ().text = "Le " + this.t.Date.ToString("dd/MM/yyyy");
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.t.competition.texture;
	}
	public void setPicture(Sprite picture)
	{
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = picture;
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
}

