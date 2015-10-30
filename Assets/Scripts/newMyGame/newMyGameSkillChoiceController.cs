using UnityEngine;
using TMPro;

public class newMyGameSkillChoiceController : SimpleButtonController 
{
	public override void OnMouseDown()
	{
		newMyGameController.instance.filterASkill (base.getId());	
	}
}

