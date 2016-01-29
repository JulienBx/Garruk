using UnityEngine;
using TMPro;

public class NewMarketSortButtonController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		NewMarketController.instance.sortButtonHandler (base.getId());	
	}
}