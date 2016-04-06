using UnityEngine;
using TMPro;

public class newMyGameDeckRenameButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayEditDeckPopUp ();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(true, false,"", this.gameObject,WordingDeck.getReference(14),WordingDeck.getReference(15));
	}
}

