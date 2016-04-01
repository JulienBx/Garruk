using UnityEngine;
using TMPro;

public class HelpCompanionGameController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		GameTutoController.instance.companionNextButtonHandler();	
	}
}

