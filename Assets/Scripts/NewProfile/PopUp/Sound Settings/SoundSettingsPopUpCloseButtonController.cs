using UnityEngine;
using TMPro;

public class SoundSettingsPopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().exitPopUp ();	
	}
}

