using UnityEngine;
using TMPro;

public class EndGamePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EndGamePopUpController> ().exitPopUp ();	
	}
}

