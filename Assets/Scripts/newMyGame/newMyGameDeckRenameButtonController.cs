using UnityEngine;
using TMPro;

public class newMyGameDeckRenameButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.displayEditDeckPopUp ();
	}
}

