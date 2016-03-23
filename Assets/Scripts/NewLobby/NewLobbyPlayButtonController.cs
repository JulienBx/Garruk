using UnityEngine;
using TMPro;

public class NewLobbyPlayButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewLobbyController.instance.playHandler ();
	}
}

