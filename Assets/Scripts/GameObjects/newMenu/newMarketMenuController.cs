using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newMarketMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewMarketController.instance.initialization ());
		base.Start ();
	}
	
}

