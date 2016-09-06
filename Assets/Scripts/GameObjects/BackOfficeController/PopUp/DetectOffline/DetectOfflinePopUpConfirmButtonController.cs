using UnityEngine;
using TMPro;

public class DetectOfflinePopUpConfirmButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DetectOfflinePopUpController> ().exitPopUp ();	
	}
}

