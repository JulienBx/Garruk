using UnityEngine;
using TMPro;

public class CheckPasswordPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		this.reset ();
		gameObject.transform.parent.GetComponent<CheckPasswordPopUpController> ().exitPopUp ();	
	}
}

