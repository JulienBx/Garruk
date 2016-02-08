using UnityEngine;
using TMPro;

public class EmailNonActivatedPopUpCancelButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EmailNonActivatedPopUpController> ().exitPopUp ();	
	}
}

