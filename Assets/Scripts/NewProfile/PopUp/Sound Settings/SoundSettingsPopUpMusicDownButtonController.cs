using UnityEngine;
using TMPro;

public class SoundSettingsPopUpMusicDownButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().musicDownHandler();
	}
}

