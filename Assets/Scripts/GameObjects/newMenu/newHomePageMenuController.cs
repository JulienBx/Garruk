using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newHomePageMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewHomePageController.instance.initialization ());
		base.Start ();
	}

}

