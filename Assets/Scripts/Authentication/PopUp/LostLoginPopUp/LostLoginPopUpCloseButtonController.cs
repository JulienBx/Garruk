using UnityEngine;
using TMPro;

public class LostLoginPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LostLoginPopUpController> ().exitPopUp ();	
	}
}

