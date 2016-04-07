using UnityEngine;
using TMPro;

public class NewProfileEditInformationsButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayEditInformationsPopUp ();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingProfile.getReference(41),WordingProfile.getReference(42));
	}
}

