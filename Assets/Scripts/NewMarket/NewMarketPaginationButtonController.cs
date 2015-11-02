using UnityEngine;
using TMPro;

public class NewMarketPaginationButtonController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		NewMarketController.instance.paginationHandler (base.getId());	
	}
}

