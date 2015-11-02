using UnityEngine;
using TMPro;

public class newMyGameSkillChoiceController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		newMyGameController.instance.filterASkill (base.getId());	
	}
}

