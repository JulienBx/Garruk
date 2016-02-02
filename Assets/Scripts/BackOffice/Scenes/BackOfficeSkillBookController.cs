using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class SkillBookBackOfficeController : BackOfficeController
{
	public override void sceneReturnPressed()
	{
		NewSkillBookController.instance.returnPressed ();
	}
	public override void sceneEscapePressed()
	{
		NewSkillBookController.instance.escapePressed ();
	}
	public override void resizeAll()
	{
		base.resize ();
		NewSkillBookController.instance.cleanSkills ();
		NewSkillBookController.instance.cleanContents ();
		NewSkillBookController.instance.resize ();
		NewSkillBookController.instance.initializeSkills ();
		NewSkillBookController.instance.initializeTabContents ();
	}
	public override void sceneCloseAllPopUp()
	{
		NewSkillBookController.instance.closeAllPopUp ();
	}
}

