using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());	
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(base.getId()),WordingCardTypes.getDescription(base.getId()));
	}
}

