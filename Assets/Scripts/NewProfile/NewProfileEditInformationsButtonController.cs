using UnityEngine;
using TMPro;

public class NewProfileEditInformationsButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayEditInformationsPopUp ();
	}
}

