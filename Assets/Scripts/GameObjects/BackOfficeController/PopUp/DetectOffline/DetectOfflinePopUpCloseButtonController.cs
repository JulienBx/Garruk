using UnityEngine;
using TMPro;

public class DetectOfflinePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DetectOfflinePopUpController> ().exitPopUp ();	
	}
}

