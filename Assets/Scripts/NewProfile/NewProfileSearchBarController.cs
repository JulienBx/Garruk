using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewProfileSearchBarController : InputTextController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.searchingUsers ();
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewProfileController.instance.mouseOnSearchBar (value);
	}
	public override void OnMouseOver()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseOver();
		}
		NewProfileController.instance.mouseOnSearchBar (true);
	}
	public override void OnMouseExit()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseExit();
		}
		NewProfileController.instance.mouseOnSearchBar (false);
	}
}

