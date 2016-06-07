using UnityEngine;
using TMPro;

public class newMyGameDeckRenameButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		if (HelpController.instance.canAccess (-1)) {
			newMyGameController.instance.displayEditDeckPopUp ();
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(14),WordingDeck.getReference(15));
	}
}

