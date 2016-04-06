using UnityEngine;
using TMPro;

public class newMyGameDeckDeletionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayDeleteDeckPopUp ();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(true, false,"", this.gameObject,WordingDeck.getReference(16),WordingDeck.getReference(17));
	}
}

