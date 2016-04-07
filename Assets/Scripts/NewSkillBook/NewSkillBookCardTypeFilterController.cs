using UnityEngine;
using TMPro;

public class NewSkillBookCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction ()
	{
		NewSkillBookController.instance.cardTypeFilterHandler (base.getId());
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(base.getId()),WordingCardTypes.getDescription(base.getId()));
	}
}

