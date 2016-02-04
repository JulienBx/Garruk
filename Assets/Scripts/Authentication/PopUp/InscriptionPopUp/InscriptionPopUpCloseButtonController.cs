using UnityEngine;
using TMPro;

public class InscriptionPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionPopUpController> ().exitPopUp ();	
	}
}

