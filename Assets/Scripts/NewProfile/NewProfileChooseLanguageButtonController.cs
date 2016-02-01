using UnityEngine;
using TMPro;

public class NewProfileChooseLanguageButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayChooseLanguagePopUp();
	}
}

