using UnityEngine;
using TMPro;

public class NewLobbyPlayButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		if(TutorialObjectController.instance.canAccess())
		{
			NewLobbyController.instance.playHandler ();
		}
	}
}

