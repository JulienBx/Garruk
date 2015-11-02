using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewMarketSkillSearchBarController : InputTextController
{	
	public override void mainInstruction()
	{
		NewMarketController.instance.searchingSkill ();
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewMarketController.instance.mouseOnSearchBar (value);
	}
	public override void OnMouseOver()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseOver();
		}
		NewMarketController.instance.mouseOnSearchBar (true);
	}
	public override void OnMouseExit()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseExit();
		}
		NewMarketController.instance.mouseOnSearchBar (false);
	}
}

