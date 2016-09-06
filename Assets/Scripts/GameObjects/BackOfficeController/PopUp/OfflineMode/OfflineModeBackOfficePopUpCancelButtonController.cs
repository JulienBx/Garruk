using UnityEngine;
using TMPro;

public class OfflineModeBackOfficePopUpCancelButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<OfflineModeBackOfficePopUpController> ().exitPopUp ();	
	}
}

