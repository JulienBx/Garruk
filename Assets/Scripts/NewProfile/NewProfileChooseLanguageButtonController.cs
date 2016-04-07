using UnityEngine;
using TMPro;

public class NewProfileChooseLanguageButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayChooseLanguagePopUp();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingProfile.getReference(43),WordingProfile.getReference(44));
	}
}

