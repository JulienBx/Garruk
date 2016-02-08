using UnityEngine;
using TMPro;

public class InscriptionFacebookPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionFacebookPopUpController> ().exitPopUp ();	
	}
}

