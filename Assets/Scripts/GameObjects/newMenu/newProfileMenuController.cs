using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newProfileMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewProfileController.instance.initialization ());
		base.Start ();
	}
	
}

