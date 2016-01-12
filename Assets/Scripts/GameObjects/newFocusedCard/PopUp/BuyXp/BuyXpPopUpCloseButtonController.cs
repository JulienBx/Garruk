using UnityEngine;
using TMPro;

public class BuyXpPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<BuyXpPopUpController> ().exitPopUp ();	
	}
}

