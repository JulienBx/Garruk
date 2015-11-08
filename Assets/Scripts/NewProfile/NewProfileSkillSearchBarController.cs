using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewSkillBookSkillSearchBarController : InputTextController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.searchingSkill ();
	}
	public override void setIsHovered(bool value)
	{
		base.setIsHovered (value);
		NewSkillBookController.instance.mouseOnSearchBar (value);
	}
	public override void OnMouseOver()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseOver();
		}
		NewSkillBookController.instance.mouseOnSearchBar (true);
	}
	public override void OnMouseExit()
	{
		if(!base.getIsSelected())
		{
			base.OnMouseExit();
		}
		NewSkillBookController.instance.mouseOnSearchBar (false);
	}
}

