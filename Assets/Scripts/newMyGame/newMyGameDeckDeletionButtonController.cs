using UnityEngine;
using TMPro;

public class newMyGameDeckDeletionButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		if (HelpController.instance.canAccess (-1)) {
			newMyGameController.instance.displayDeleteDeckPopUp ();
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(16),WordingDeck.getReference(17));
	}
}

