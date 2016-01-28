using UnityEngine;
using TMPro;

public class BuyPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<BuyPopUpController> ().buyHandler ();	
	}
}

