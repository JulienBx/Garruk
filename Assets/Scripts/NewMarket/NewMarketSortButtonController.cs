using UnityEngine;
using TMPro;

public class NewMarketSortButtonController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		NewMarketController.instance.sortButtonHandler (base.getId());	
	}
}