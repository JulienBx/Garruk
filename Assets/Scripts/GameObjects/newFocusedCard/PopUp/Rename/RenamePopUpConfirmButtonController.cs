using UnityEngine;
using TMPro;

public class RenamePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<RenamePopUpController> ().renameHandler ();	
	}
}

