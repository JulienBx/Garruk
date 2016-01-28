using UnityEngine;
using TMPro;

public class SellPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SellPopUpController> ().sellHandler ();	
	}
}

