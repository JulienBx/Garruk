using UnityEngine;
using TMPro;

public class InvitationPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InvitationPopUpController> ().quitPopUp ();
	}
}

