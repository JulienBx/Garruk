using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMyGameMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (newMyGameController.instance.initialization ());
		base.Start ();
	}
	
}

