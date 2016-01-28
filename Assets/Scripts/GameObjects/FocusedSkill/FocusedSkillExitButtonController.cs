using UnityEngine;
using TMPro;

public class FocusedSkillExitButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		this.gameObject.transform.parent.GetComponent<FocusedSkillController>().exit();
	}
}

