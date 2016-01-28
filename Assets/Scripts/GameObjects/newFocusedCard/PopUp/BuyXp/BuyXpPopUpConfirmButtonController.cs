using UnityEngine;
using TMPro;

public class BuyXpPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<BuyXpPopUpController> ().buyXpHandler ();	
	}
}

