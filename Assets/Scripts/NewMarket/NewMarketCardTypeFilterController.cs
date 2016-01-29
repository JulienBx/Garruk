using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());	
	}
}

