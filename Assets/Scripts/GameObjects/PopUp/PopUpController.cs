using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PopUpController : MonoBehaviour 
{
	
	private int Id;
	private bool isHovering;
	private bool isNotification;
	private bool isNews;
	private bool isCompetition;
	private bool isResult;
	private bool isFriend;
	private bool isFriendsRequest;
	
	
	void Awake()
	{
		this.isHovering = false;
	}
	
	public void OnMouseOver()
	{
		if(!isHovering)
		{
			this.startHoveringPopUp();
			this.isHovering=true;
		}
	}
	public void OnMouseExit()
	{
		if(isHovering)
		{
			this.endHoveringPopUp();
			this.isHovering=false;
		}
	}
	public virtual void startHoveringPopUp()
	{
	}
	public virtual void endHoveringPopUp()
	{
	}
	public void setIsNotification(bool value)
	{
		this.isNotification = value;
	}
	public bool getIsNotification()
	{
		return isNotification;
	}
	public void setIsNews(bool value)
	{
		this.isNews = value;
	}
	public bool getIsNews()
	{
		return isNews;
	}
	public bool getIsFriend()
	{
		return isFriend;
	}
	public void setIsFriend(bool value)
	{
		this.isFriend = value;
	}
	public void setIsCompetition(bool value)
	{
		this.isCompetition = value;
	}
	public bool getIsCompetition()
	{
		return isCompetition;
	}
	public void setIsResult(bool value)
	{
		this.isResult = value;
	}
	public bool getIsResult()
	{
		return isResult;
	}
	public void setIsFriendsRequest(bool value)
	{
		this.isFriendsRequest = value;
	}
	public bool getIsFriendsRequest()
	{
		return isFriendsRequest;
	}
	public void setId(int Id)
	{
		this.Id = Id;
	}
	public int getId()
	{
		return Id;
	}
}

