using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class NewEndGameQuitButtonController : SimpleButtonController 
{	
	public override void mainInstruction()
	{
		NewEndGameController.instance.quitEndGameHandler ();
	}
}

