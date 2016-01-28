using UnityEngine;
using TMPro;

public class AddCreditsPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AddCreditsPopUpController> ().addCreditsHandler ();
	}
}

