using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class newLobbyMenuController : newMenuController
{
	
	public override void Start ()
	{
		StartCoroutine (NewLobbyController.instance.initialization ());
		base.Start ();
	}
	
}

