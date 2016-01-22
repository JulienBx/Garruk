using UnityEngine;
using TMPro;

public class SearchUsersPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SearchUsersPopUpController> ().exitPopUp ();	
	}
}

