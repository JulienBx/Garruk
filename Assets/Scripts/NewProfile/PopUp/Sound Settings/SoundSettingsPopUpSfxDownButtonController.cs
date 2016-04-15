using UnityEngine;
using TMPro;

public class SoundSettingsPopUpSfxDownButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().sfxDownHandler();
	}
}

