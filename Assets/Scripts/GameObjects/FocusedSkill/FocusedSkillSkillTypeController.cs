using UnityEngine;
using TMPro;

public class FocusedSkillSkillTypeController : TextToHoverController 
{
	private int skillTypeId;

	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingSkillTypes.getName(this.skillTypeId),WordingSkillTypes.getDescription(this.skillTypeId));
	}
	public void setSkillType(int skillTypeId)
	{
		this.skillTypeId=skillTypeId;
	}
}

