using UnityEngine;

public class ProfileFriendsRequestsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerFriendsRequests (this.id);
	}
}

