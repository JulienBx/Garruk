using UnityEngine;
using TMPro;

public class NewMarketCardTypeFilterController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		NewMarketController.instance.cardTypeFilterHandler (base.getId());	
	}
}

