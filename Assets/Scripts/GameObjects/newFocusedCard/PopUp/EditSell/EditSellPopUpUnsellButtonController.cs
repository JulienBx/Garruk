using UnityEngine;
using TMPro;

public class EditSellPopUpUnsellButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditSellPopUpController> ().unsellHandler ();	
	}
}

