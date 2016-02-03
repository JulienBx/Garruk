using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class OnlineFriendController : MonoBehaviour 
{
	
	public User u;
	public int Id;

	private bool isHovering;
	
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			this.startHovering();
			gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	public virtual void startHovering()
	{
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			this.endHovering();
			this.setOnlineStatus ();
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	public virtual void endHovering()
	{
	}
	void OnMouseDown()
	{
		
	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.u.Username;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnThumbPicture(this.u.IdProfilePicture);
		this.setOnlineStatus ();
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
			if(this.u.OnlineStatus==2)
			{
				gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(182f/255f,0f,0f);
			}
			else if(this.u.OnlineStatus==1)
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

