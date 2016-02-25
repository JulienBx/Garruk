using UnityEngine;
using TMPro;

public class PermuteCardPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PermuteCardPopUpController> ().exitPopUp ();	
	}
}

