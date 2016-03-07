using UnityEngine;
using TMPro;

public class FocusedSkillExitButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		this.gameObject.transform.parent.GetComponent<FocusedSkillController>().exit();
	}
}

