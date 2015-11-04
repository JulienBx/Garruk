using UnityEngine;
using TMPro;

public class NewProfileEditInformationsButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.displayEditInformationsPopUp ();
	}
}

