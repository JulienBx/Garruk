using UnityEngine;
using TMPro;

public class AuthenticationChangePasswordPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AuthenticationChangePasswordPopUpController> ().exitPopUp ();	
	}
}

