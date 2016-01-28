using UnityEngine;
using TMPro;

public class SoldCardPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoldCardPopUpController> ().exitPopUp ();	
	}
}

