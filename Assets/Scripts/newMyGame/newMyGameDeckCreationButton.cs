using UnityEngine;
using TMPro;

public class newMyGameDeckCreatioButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayNewDeckPopUp();
	}
}

