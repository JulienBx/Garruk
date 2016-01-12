using UnityEngine;
using TMPro;

public class EditSellPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditSellPopUpController> ().exitPopUp ();	
	}
}

