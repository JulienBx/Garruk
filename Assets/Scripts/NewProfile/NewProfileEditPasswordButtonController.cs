using UnityEngine;
using TMPro;

public class NewProfileEditPasswordButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayCheckPasswordPopUp ();
	}
}

