using UnityEngine;

public class ProfileTrophiesPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerTrophies (this.id);
	}
}

