using UnityEngine;
using TMPro;

public class TutorialObjectQuitButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		TutorialObjectController.instance.quitButtonHandler ();
	}
}