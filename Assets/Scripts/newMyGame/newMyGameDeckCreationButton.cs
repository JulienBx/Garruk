using UnityEngine;
using TMPro;

public class newMyGameDeckCreatioButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		if(HelpController.instance.canAccess(3001)){
			newMyGameController.instance.displayNewDeckPopUp();
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingDeck.getReference(20),WordingDeck.getReference(21));
	}
}

