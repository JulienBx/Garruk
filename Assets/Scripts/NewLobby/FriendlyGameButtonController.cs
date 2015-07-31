using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class FriendlyGameButtonController : LobbyButtonController
{
	
	public override void drawBackSide()
	{
		gameObject.transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text="Jouer";
	}
	public override void drawFrontSide()
	{
		gameObject.transform.FindChild("Button").FindChild("Title").GetComponent<TextMeshPro>().text=null;
	}
}

