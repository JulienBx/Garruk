using UnityEngine;
using TMPro;

public class SoundSettingsPopUpMusicUpButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().musicUpHandler();
	}
}

