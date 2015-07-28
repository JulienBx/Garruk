using UnityEngine;

public class HomePageNotificationsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewHomePageController.instance.paginationHandlerNotifications (this.id);
	}
}

