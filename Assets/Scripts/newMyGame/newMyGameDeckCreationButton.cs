using UnityEngine;
using TMPro;

public class newMyGameDeckCreatioButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayNewDeckPopUp();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(true, false, "",this.gameObject,WordingDeck.getReference(20),WordingDeck.getReference(21));
	}
}

