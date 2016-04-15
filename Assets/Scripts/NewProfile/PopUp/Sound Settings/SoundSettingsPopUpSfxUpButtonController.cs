using UnityEngine;
using TMPro;

public class SoundSettingsPopUpSfxUpButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().sfxUpHandler();
	}
}

