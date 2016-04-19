using UnityEngine;
using TMPro;

public class NewProfileSoundSettingsButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displaySoundSettingsPopUp();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingProfile.getReference(47),WordingProfile.getReference(48));
	}
}

