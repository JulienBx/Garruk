using UnityEngine;
using TMPro;

public class EditInformationsPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditInformationsPopUpController> ().exitPopUp ();	
	}
}

