using UnityEngine;
using TMPro;

public class DisconnectPopUpQuitButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DisconnectPopUpController> ().quitHandler ();
	}
}

