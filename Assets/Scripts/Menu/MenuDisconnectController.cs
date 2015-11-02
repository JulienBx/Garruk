using UnityEngine;

public class MenuDisconnectController : SpriteButtonController
{
	public override void mainInstruction()
	{
		MenuController.instance.displayDisconnectedPopUp ();
	}
}

