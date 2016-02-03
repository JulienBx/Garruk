using UnityEngine;
using TMPro;

public class ErrorPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ErrorPopUpController> ().exitPopUp ();	
	}
}

