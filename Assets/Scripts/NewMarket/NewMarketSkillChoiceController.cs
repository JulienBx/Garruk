using UnityEngine;
using TMPro;

public class NewMarketSkillChoiceController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewMarketController.instance.filterASkill (base.getId());	
	}
	public void OnMouseDown()
	{
		base.OnMouseUp ();
	}
}

