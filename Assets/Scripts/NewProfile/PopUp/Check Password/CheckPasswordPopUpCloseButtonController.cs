using UnityEngine;
using TMPro;

public class CheckPasswordPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<CheckPasswordPopUpController> ().exitPopUp ();	
	}
}

