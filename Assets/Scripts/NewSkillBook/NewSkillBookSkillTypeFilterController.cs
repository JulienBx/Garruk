using UnityEngine;
using TMPro;

public class NewSkillBookSkillTypeFilterController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.skillTypeFilterHandler (base.getId());	
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingSkillTypes.getName(base.getId()),WordingSkillTypes.getDescription(base.getId()));
	}
}

