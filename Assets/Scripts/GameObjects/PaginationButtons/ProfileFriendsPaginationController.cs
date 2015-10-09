using UnityEngine;

public class ProfileFriendsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerFriends (this.id);
	}
}

