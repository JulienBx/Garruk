using UnityEngine;
using TMPro;

public class NewMarketSortButtonController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		NewMarketController.instance.sortButtonHandler (base.getId());	
	}
}