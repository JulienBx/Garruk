using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMyGameSkillSearchBarController : InputTextController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.searchingSkill ();
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		newMyGameController.instance.mouseOnSearchBar (value);
	}
	public override void OnMouseOver()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseOver();
		}
		newMyGameController.instance.mouseOnSearchBar (true);
	}
	public override void OnMouseExit()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseExit();
		}
		newMyGameController.instance.mouseOnSearchBar (false);
	}
}

