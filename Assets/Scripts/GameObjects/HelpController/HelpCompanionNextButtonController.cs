using UnityEngine;
using TMPro;

public class HelpCompanionNextButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		HelpController.instance.companionNextButtonHandler();	
	}
}

