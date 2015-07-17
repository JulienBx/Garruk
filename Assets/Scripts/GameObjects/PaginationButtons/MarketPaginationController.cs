using UnityEngine;

public class MarketPaginationController : PaginationController
{
	void OnMouseDown()
	{
		base.isActive=!base.isActive;
		base.setSprite ();
		NewMarketController.instance.paginationHandler (this.id);
	}
}

