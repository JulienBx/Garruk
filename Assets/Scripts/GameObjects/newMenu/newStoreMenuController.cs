using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newStoreMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewStoreController.instance.initialization ());
		base.Start ();
	}
	
}

