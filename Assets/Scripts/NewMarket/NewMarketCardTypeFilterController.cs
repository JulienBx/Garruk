using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction ()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(base.getId()),WordingCardTypes.getDescription(base.getId()));
	}
}

