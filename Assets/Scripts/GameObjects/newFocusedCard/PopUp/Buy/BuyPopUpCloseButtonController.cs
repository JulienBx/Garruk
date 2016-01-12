using UnityEngine;
using TMPro;

public class BuyPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<BuyPopUpController> ().exitPopUp ();	
	}
}

