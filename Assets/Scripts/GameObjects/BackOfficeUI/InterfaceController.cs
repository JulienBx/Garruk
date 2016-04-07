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

	
	public virtual void Awake()
	{
		this.reset ();
	}
	public virtual void reset()
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
				if(!isHovered && !ApplicationDesignRules.isMobileScreen)
				{
					this.setHoveredState();
					this.setIsHovered(true);
					this.showToolTip();
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
				if(isHovered && !ApplicationDesignRules.isMobileScreen)
				{
					this.setInitialState();
					this.setIsHovered(false);
					this.hideToolTip();
				}
			}
		}
	}
	public virtual void OnMouseUp()
	{
		if(this.isActive)
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				if(!BackOfficeController.instance.getIsSwiping() && !BackOfficeController.instance.getIsToolTipDisplayed())
				{
					this.mainInstruction();	
				}
				this.hideToolTip();
			}
			else
			{
				this.hideToolTip();
				if(this.isHovered)
				{
					this.mainInstruction();
				}
			}
		}
	}
	public virtual void OnMouseDown()
	{
		if(ApplicationDesignRules.isMobileScreen && !BackOfficeController.instance.getIsScrolling())
		{
			this.showToolTip();
		}
	}
	public virtual void mainInstruction()
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
	public virtual void setForbiddenState()
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
	public virtual void setIsActive(bool value)
	{
		this.isActive = value;
	}
	public void setIsSelected(bool value)
	{
		this.isSelected = value;
		this.hideToolTip();
	}
	public bool getIsSelected()
	{
		return this.isSelected;
	}
	public bool getIsHovered()
	{
		return this.isHovered;
	}
	public bool getIsActive()
	{
		return this.isActive;
	}
	public virtual void showToolTip()
	{
	}
	public void hideToolTip()
	{
		BackOfficeController.instance.hideToolTip();
	}
}

