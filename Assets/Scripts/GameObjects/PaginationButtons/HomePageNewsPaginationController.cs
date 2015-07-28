using UnityEngine;

public class HomePageNewsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewHomePageController.instance.paginationHandlerNews (this.id);
	}
}

