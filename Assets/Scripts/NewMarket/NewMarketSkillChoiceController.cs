using UnityEngine;
using TMPro;

public class NewMarketSkillChoiceController : SimpleButtonController 
{
	public override void OnMouseDown()
	{
		NewMarketController.instance.filterASkill (base.getId());	
	}
}

