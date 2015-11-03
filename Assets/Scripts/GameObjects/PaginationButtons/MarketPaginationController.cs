using UnityEngine;

public class MarketPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		base.isActive=!base.isActive;
		base.setSprite ();
		NewMarketController.instance.paginationHandler ();
	}
}

