using UnityEngine;
using TMPro;

public class SelectPicturePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		this.reset ();
		gameObject.transform.parent.GetComponent<SelectPicturePopUpController> ().exitPopUp ();	
	}
}

