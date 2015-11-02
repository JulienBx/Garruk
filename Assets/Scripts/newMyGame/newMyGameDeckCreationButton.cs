using UnityEngine;
using TMPro;

public class newMyGameDeckCreatioButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayNewDeckPopUp();
	}
}

