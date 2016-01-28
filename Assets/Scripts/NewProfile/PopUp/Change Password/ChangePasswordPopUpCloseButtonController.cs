using UnityEngine;
using TMPro;

public class ChangePasswordPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ChangePasswordPopUpController> ().exitPopUp ();	
	}
}

