using UnityEngine;

public class MenuAdminController : SpriteButtonController
{
	public override void OnMouseDown()
	{
		MenuController.instance.adminBoardLink ();
	}
}

