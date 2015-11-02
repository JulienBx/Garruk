using UnityEngine;

public class MenuAdminController : SpriteButtonController
{
	public override void mainInstruction()
	{
		MenuController.instance.adminBoardLink ();
	}
}

