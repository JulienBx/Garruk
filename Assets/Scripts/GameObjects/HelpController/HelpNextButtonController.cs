using UnityEngine;
using TMPro;

public class HelpNextButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		HelpController.instance.nextButtonHandler();	
	}
}

