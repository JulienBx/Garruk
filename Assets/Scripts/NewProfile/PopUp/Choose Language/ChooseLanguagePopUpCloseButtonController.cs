using UnityEngine;
using TMPro;

public class ChooseLanguagePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ChooseLanguagePopUpController> ().exitPopUp ();	
	}
}

