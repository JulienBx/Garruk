using UnityEngine;
using TMPro;

public class NewSkillBookSkillChoiceController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewSkillBookController.instance.filterASkill (base.getId());	
	}
	public void OnMouseDown()
	{
		base.OnMouseUp ();
	}
}

