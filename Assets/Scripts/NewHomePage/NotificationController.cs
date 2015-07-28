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
	

	void OnMouseOver()
	{
	
	}
	void OnMouseExit()
	{

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
		gameObject.transform.FindChild ("Notification").GetComponent<TextMeshPro> ().text = this.n.Content;
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

