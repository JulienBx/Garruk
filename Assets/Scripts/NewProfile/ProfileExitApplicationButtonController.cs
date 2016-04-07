using UnityEngine;
using TMPro;

public class NewProfileExitApplicationButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		BackOfficeController.instance.displayDisconnectedPopUp();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingProfile.getReference(45),WordingProfile.getReference(46));
	}
}

