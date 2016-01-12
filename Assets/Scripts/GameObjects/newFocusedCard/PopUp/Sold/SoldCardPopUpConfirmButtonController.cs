using UnityEngine;
using TMPro;

public class SoldPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoldCardPopUpController> ().exitPopUp ();	
	}
}

