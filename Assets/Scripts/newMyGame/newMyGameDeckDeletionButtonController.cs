using UnityEngine;
using TMPro;

public class newMyGameDeckDeletionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeleteDeckPopUp ();
	}
}

