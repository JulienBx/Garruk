using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewsController : MonoBehaviour 
{
	
	public DisplayedNews n;
	
	private int Id;
	
	
	void OnMouseOver()
	{
		
	}
	void OnMouseExit()
	{
		
	}
	void OnMouseDown()
	{
		
	}
	public void setNews(DisplayedNews news)
	{
		this.n = news;
	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.n.User.Username;
		gameObject.transform.FindChild ("News").GetComponent<TextMeshPro> ().text = this.n.Content;
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

