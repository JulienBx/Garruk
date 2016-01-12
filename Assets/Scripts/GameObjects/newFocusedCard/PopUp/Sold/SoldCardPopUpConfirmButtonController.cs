using UnityEngine;
using TMPro;

public class SoldCardPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoldCardPopUpController> ().exitPopUp ();	
	}
}

