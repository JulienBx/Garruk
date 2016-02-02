using UnityEngine;
using TMPro;

public class DisconnectPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DisconnectPopUpController> ().exitPopUp ();	
	}
}

