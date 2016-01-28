using UnityEngine;
using TMPro;

public class AddCreditsPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AddCreditsPopUpController> ().exitPopUp ();	
	}
}

