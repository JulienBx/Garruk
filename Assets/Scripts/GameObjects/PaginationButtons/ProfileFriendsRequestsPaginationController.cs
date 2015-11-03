using UnityEngine;

public class ProfileFriendsRequestsPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerFriendsRequests (this.id);
	}
}

