using UnityEngine;
using TMPro;

public class AuthenticationMessagePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AuthenticationMessagePopUpController> ().exitPopUp ();	
	}
}

