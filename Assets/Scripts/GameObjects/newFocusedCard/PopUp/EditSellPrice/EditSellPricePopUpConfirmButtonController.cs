using UnityEngine;
using TMPro;

public class EditSellPricePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditSellPricePopUpController> ().editSellPriceHandler ();	
	}
}

