using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ResultController : MonoBehaviour 
{
	
	public PlayerResult pR;
	
	private int Id;
	private bool isHovering;
	
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			NewLobbyController.instance.startHoveringResult (this.Id);
			//gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			//gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			//gameObject.transform.FindChild("News").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			NewLobbyController.instance.endHoveringResult ();
			//gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			//gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			//gameObject.transform.FindChild("News").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		
	}
	public void setResult(PlayerResult pR)
	{
		this.pR = pR;
	}
	public void show()
	{
		//gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.n.User.Username;
		//gameObject.transform.FindChild ("News").GetComponent<TextMeshPro> ().text = this.n.News.Description;
		//gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.n.User.texture;
	}
	public void setPicture(Sprite picture)
	{
		//gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = picture;
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
}

