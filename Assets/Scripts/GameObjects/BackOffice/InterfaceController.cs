using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class InterfaceController : MonoBehaviour 
{	
	private int id;
	private bool isSelected;
	private bool isHovered;
	private bool isActive;
	
	void Awake()
	{
		this.reset ();
	}
	public void reset()
	{
		this.isHovered = false;
		this.isActive = true;
		this.isSelected = false;
		this.setInitialState ();
	}
	public virtual void OnMouseOver()
	{
		if(isActive)
		{
			if(!isSelected)
			{
				if(!isHovered)
				{
					this.setHoveredState();
					this.setIsHovered(true);
				}
			}
		}
	}
	public virtual void OnMouseExit()
	{
		if(isActive)
		{	
			if(!isSelected)
			{
				if(isHovered)
				{
					this.setInitialState();
					this.setIsHovered(false);
				}
			}
		}
	}
	public virtual void OnMouseDown()
	{
	}
	public virtual void setIsHovered(bool value)
	{
		this.isHovered = value;
	}
	public virtual void setHoveredState()
	{
	}
	public virtual void setInitialState()
	{
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public int getId()
	{
		return this.id;
	}
	public void setIsActive(bool value)
	{
		this.isActive = value;
	}
	public void setIsSelected(bool value)
	{
		this.isSelected = value;
	}
	public bool getIsSelected()
	{
		return this.isSelected;
	}
	public bool getIsHovered()
	{
		return this.isHovered;
	}
}

