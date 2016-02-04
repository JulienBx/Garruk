using UnityEngine;
using TMPro;

public class PasswordResetPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PasswordResetPopUpController> ().exitPopUp ();	
	}
}

