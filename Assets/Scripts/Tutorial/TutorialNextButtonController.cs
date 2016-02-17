using UnityEngine;
using TMPro;


public class TutorialNextButtonController: SimpleButtonController 
{
	public override void mainInstruction()
	{
		TutorialController.instance.nextButtonHandler();
	}
}




