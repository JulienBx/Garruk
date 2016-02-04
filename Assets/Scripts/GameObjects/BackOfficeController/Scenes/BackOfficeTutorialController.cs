using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class BackOfficeTutorialController : BackOfficeController
{
	public override void resizeAll()
	{
		base.resize ();
		TutorialController.instance.resize ();
	}
}

