using UnityEngine;
using TMPro;

public class newMyGameDeckRenameButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayEditDeckPopUp ();
	}
}

