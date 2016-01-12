using UnityEngine;
using TMPro;

public class ConnectionBonusPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ConnectionBonusPopUpController> ().exitPopUp ();	
	}
}

