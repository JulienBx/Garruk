using UnityEngine;
using TMPro;

public class SoldPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoldCardPopUpController> ().exitPopUp ();	
	}
}

