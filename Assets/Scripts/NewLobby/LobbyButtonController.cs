using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyButtonController : MonoBehaviour 
{
	
	private Sprite defaultSprite;
	private bool isClickable;
	private bool isHovering;
	private bool isFrontSideDisplayed;
	private int id;
	
	void Awake()
	{
		this.isClickable = false;
		this.isHovering = false;
		this.isFrontSideDisplayed = true;
	}
	public void setDefaultSprite(Sprite sprite)
	{
		this.defaultSprite = sprite;
		if(this.isFrontSideDisplayed)
		{
			gameObject.transform.FindChild("Button").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite=this.defaultSprite;
		}
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public void setClickable(bool value)
	{
		this.isClickable = value;
		if(!value)
		{
			gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
			gameObject.transform.FindChild("Button").FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
		}
		else
		{
			gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Button").FindChild("Picture").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseOver()
	{
		if(isClickable)
		{
			if(!isHovering)
			{
				this.isHovering=true;
				NewLobbyController.instance.startFriendlyGameButtonHovering(this.id);
			}
		}
	}
	void OnMouseExit()
	{
		if(isClickable)
		{
			if(isHovering)
			{
				this.isHovering=false;
				NewLobbyController.instance.endFriendlyGameButtonHovering(this.id);
			}
		}
	}
	void OnMouseDown()
	{
		if(isClickable)
		{
			NewLobbyController.instance.joinGame(this.id);	
		}
	}
	public void askForBackSide()
	{
		if(this.isFrontSideDisplayed)
		{
			this.isFrontSideDisplayed = false;
			gameObject.transform.FindChild("Button").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = null;
			this.drawBackSide();
		}
	}
	public virtual void drawBackSide()
	{
	}
	public void askForFrontSide()
	{
		if(!this.isFrontSideDisplayed)
		{
			this.isFrontSideDisplayed = true;
			gameObject.transform.FindChild("Button").FindChild ("Picture").GetComponent<SpriteRenderer> ().sprite = this.defaultSprite;
			this.drawFrontSide();
		}
	}
	public virtual void drawFrontSide()
	{
	}
}

