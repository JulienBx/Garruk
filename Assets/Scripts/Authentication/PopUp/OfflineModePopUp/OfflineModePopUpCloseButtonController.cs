using UnityEngine;
using TMPro;

public class OfflineModePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<OfflineModePopUpController> ().exitPopUp ();	
	}
}

