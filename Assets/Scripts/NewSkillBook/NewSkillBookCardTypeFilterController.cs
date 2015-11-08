using UnityEngine;
using TMPro;

public class NewSkillBookCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewSkillBookController.instance.cardTypeFilterHandler (base.getId());	
	}
}

