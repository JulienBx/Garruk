using UnityEngine;
using TMPro;

public class PutOnMarketPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PutOnMarketPopUpController> ().exitPopUp ();	
	}
}

