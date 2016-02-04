using UnityEngine;
using TMPro;

public class AccountCreatedPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AccountCreatedPopUpController> ().exitPopUp ();	
	}
}

