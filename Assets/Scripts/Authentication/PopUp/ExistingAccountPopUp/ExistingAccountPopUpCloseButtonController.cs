using UnityEngine;
using TMPro;

public class ExistingAccountPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ExistingAccountPopUpController> ().exitPopUp ();	
	}
}

