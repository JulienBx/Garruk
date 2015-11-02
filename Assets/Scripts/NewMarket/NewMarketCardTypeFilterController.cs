using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());	
	}
}

