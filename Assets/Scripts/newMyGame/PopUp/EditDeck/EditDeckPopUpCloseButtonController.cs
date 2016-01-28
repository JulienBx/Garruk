using UnityEngine;
using TMPro;

public class EditDeckPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditDeckPopUpController> ().exitPopUp ();	
	}
}

