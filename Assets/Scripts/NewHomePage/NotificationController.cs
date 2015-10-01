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
			this.setOnlineStatus ();
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Notification").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{

	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.n.SendingUser.Username;
		gameObject.transform.FindChild ("Notification").GetComponent<TextMeshPro> ().text = this.n.Notification.Description;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.n.SendingUser.texture;
		this.setOnlineStatus ();

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
	public void setOnlineStatus()
	{
		if(!this.isHovering)
		{
			if(this.n.SendingUser.OnlineStatus==2)
			{
				gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(182f/255f,0f,0f);
			}
			else if(this.n.SendingUser.OnlineStatus==1)
			{
				gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(0f,182f/255f,29f/255f);
			}
			else
			{
				gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			}
		}
	}
}

