using UnityEngine;
using TMPro;

public class ProfileMessagePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ProfileMessagePopUpController> ().exitPopUp ();	
	}
}

