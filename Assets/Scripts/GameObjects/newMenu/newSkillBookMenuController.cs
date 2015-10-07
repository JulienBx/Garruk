using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newSkillBookMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewSkillBookController.instance.initialization ());
		base.Start ();
	}
	
}

