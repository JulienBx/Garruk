using UnityEngine;
using TMPro;

public class NewSkillBookSkillTypeFilterController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.skillTypeFilterHandler (base.getId());	
	}
}

