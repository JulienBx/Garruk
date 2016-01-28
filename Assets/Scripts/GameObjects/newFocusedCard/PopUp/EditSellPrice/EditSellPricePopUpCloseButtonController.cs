using UnityEngine;
using TMPro;

public class EditSellPricePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditSellPricePopUpController> ().exitPopUp ();	
	}
}

