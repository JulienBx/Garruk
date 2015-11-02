using UnityEngine;
using TMPro;

public class newMyGameDeckDeletionButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeleteDeckPopUp ();
	}
}

