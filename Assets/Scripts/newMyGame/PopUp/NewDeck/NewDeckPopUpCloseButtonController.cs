using UnityEngine;
using TMPro;

public class NewDeckPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<NewDeckPopUpController> ().exitPopUp ();	
	}
}

