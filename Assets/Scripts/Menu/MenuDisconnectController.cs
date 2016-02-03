using UnityEngine;

public class MenuDisconnectController : SpriteButtonController
{
	public override void mainInstruction()
	{
		BackOfficeController.instance.displayDisconnectedPopUp ();
	}
}

