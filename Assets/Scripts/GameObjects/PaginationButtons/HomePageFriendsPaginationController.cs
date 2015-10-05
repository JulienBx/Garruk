using UnityEngine;

public class HomePageFriendsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewHomePageController.instance.paginationHandlerFriends (this.id);
	}
}

