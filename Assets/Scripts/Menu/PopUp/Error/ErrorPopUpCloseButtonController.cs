using UnityEngine;
using TMPro;

public class ErrorPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ErrorPopUpController> ().exitPopUp ();	
	}
}

