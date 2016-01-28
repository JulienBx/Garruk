using UnityEngine;
using TMPro;

public class EditSellPopUpEditPriceButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditSellPopUpController> ().editPriceHandler ();	
	}
}

