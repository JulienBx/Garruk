using UnityEngine;

public class ProfileConfrontationsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerConfrontations (this.id);
	}
}

