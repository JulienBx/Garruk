using UnityEngine;
using TMPro;

public class EmailNonActivatedPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EmailNonActivatedPopUpController> ().exitPopUp ();	
	}
}

