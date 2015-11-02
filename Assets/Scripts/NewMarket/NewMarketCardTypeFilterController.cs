using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());	
	}
}

