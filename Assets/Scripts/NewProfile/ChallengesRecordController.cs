using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ChallengesRecordController : MonoBehaviour 
{
	
	public ChallengesRecord c;
	
	private int Id;
	private bool isHovering;
	
	
	void OnMouseOver()
	{
		if(!isHovering)
		{
			this.isHovering=true;
			//NewProfileController.instance.startHoveringFriendsRequest (this.Id);
			gameObject.transform.FindChild("PictureBorder").GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isHovering)
		{
			this.isHovering=false;
			//NewProfileController.instance.endHoveringFriend ();
			gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{

	}
	public void show()
	{
		gameObject.transform.FindChild ("Username").GetComponent<TextMeshPro> ().text = this.c.Friend.Username;
		gameObject.transform.FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.c.Friend.texture;
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

