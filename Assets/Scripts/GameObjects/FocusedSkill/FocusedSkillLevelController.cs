using UnityEngine;
using TMPro;

public class FocusedSkillLevelController : TextToHoverController 
{
	private int cardTypeId;

	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingSkillBook.getReference(19),WordingSkillBook.getReference(20));
	}
}

