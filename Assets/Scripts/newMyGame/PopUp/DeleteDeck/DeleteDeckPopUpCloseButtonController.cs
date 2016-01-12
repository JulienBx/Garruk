using UnityEngine;
using TMPro;

public class DeleteDeckPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DeleteDeckPopUpController> ().exitPopUp ();	
	}
}

