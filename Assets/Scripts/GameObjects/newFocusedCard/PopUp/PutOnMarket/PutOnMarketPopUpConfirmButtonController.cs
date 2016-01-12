using UnityEngine;
using TMPro;

public class PutOnMarketPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PutOnMarketPopUpController> ().putOnMarketHandler ();	
	}
}

