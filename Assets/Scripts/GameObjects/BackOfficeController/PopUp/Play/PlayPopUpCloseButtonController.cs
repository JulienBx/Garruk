using UnityEngine;
using TMPro;

public class PlayPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PlayPopUpController> ().quitPopUp ();
	}
}

