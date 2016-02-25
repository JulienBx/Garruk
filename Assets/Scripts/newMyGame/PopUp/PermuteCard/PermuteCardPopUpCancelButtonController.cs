using UnityEngine;
using TMPro;

public class PermuteCardPopUpCancelButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PermuteCardPopUpController> ().exitPopUp ();	
	}
}

