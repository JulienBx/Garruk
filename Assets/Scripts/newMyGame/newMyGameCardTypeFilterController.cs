using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		if(HelpController.instance.canAccess(-1))
		{
			newMyGameController.instance.cardTypeFilterHandler (base.getId());
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(base.getId()),WordingCardTypes.getDescription(base.getId()));
	}
}

