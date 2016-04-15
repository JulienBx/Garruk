using UnityEngine;
using TMPro;

public class SoundSettingsPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SoundSettingsPopUpController> ().confirmButtonHandler();
	}
}

