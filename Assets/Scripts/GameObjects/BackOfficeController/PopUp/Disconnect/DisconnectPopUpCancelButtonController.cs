using UnityEngine;
using TMPro;

public class DisconnectPopUpCancelButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DisconnectPopUpController> ().exitPopUp ();	
	}
}

