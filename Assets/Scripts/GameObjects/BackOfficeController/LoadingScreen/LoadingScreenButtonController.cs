using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LoadingScreenButtonController : SimpleButtonController 
{
	public override void mainInstruction ()
	{
		print ("toto");
		SoundController.instance.playSound(9);
        BackOfficeController.instance.loadingScreenButtonHandler();
	}
}

