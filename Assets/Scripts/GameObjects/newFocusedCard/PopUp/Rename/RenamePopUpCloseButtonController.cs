using UnityEngine;
using TMPro;

public class RenamePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<RenamePopUpController> ().exitPopUp ();	
	}
}

