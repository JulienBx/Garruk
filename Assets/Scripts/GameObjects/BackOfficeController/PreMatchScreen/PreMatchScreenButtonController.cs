using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class PreMatchScreenButtonController : SimpleButtonController 
{
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(9);
		PhotonController.instance.leaveRandomRoomHandler();
	}
}

