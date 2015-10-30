using UnityEngine;

public class MenuDisconnectController : SpriteButtonController
{
	public override void OnMouseDown()
	{
		MenuController.instance.displayDisconnectedPopUp ();
	}
}

