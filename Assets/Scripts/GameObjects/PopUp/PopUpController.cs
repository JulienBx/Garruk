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
	public void setIsCompetition(bool value)
	{
		this.isCompetition = value;
	}
	public bool getIsCompetition()
	{
		return isCompetition;
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
