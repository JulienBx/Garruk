using UnityEngine;
using TMPro;

public class NewProfileEditPasswordButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayCheckPasswordPopUp ();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingProfile.getReference(39),WordingProfile.getReference(40));
	}
}

