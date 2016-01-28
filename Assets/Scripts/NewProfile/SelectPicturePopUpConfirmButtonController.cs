using UnityEngine;
using TMPro;

public class SelectPicturePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		this.reset ();
		gameObject.transform.parent.GetComponent<SelectPicturePopUpController> ().confirmPicture ();	
	}
}

