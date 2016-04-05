using UnityEngine;
using TMPro;

public class NewProfileExitApplicationButtonController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		BackOfficeController.instance.displayDisconnectedPopUp();
	}
}

