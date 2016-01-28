using UnityEngine;
using TMPro;

public class NewLobbyPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		NewLobbyController.instance.hidePopUp ();
	}
}

