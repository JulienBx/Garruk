using UnityEngine;

public class ProfileTrophiesPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerTrophies (this.id);
	}
}

