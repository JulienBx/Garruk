using UnityEngine;
using TMPro;

public class NewProfileEditPasswordButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayCheckPasswordPopUp ();
	}
}

