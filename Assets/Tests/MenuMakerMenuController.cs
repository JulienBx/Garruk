using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MenuMakerMenuController : MenuController
{
	
	public override void Start ()
	{
		base.Start ();
	}
	public override void sceneReturnPressed()
	{
		//NewSkillBookController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		//NewSkillBookController.instance.escapePressed ();
	}
	public override void sceneCloseAllPopUp()
	{
		//NewSkillBookController.instance.closeAllPopUp ();
	}
	
}

