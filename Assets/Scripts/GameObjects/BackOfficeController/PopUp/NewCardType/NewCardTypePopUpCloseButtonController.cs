using UnityEngine;
using TMPro;

public class NewCardTypePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<NewCardTypePopUpController> ().exitPopUp ();	
	}
}

