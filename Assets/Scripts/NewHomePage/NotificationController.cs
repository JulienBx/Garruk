using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NotificationController : MonoBehaviour 
{

	public DisplayedNotification n;
	
	private int Id;
	private bool isHovering;
	

	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			NewHomePageController.instance.startHoveringNotification (this.Id);
			gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Notification").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			NewHomePageController.instance.endHoveringNotification ();
			gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Notification").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{

	}
	public void setNotification(DisplayedNotification notification)
	{
		this.n = notification;
	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.n.SendingUser.Username;
		gameObject.transform.FindChild ("Notification").GetComponent<TextMeshPro> ().text = this.n.Notification.Description;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.n.SendingUser.texture;

		if(!this.n.Notification.IsRead)
		{
			gameObject.transform.FindChild("New").GetComponent<TextMeshPro>().text="Nouveau !";
		}
		else
		{
			gameObject.transform.FindChild("New").GetComponent<TextMeshPro>().text="";
		}
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

