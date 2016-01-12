using UnityEngine;
using TMPro;

public class SellPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SellPopUpController> ().exitPopUp ();	
	}
}

