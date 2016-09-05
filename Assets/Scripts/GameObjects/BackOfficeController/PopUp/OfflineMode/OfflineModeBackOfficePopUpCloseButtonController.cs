using UnityEngine;
using TMPro;

public class OfflineModeBackOfficePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<OfflineModeBackOfficePopUpController> ().exitPopUp ();	
	}
}

