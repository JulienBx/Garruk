using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.cardTypeFilterHandler (base.getId());
		}
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(true, false,"",this.gameObject,WordingCardTypes.getName(base.getId()),WordingCardTypes.getDescription(base.getId()));
	}
}

