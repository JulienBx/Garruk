using UnityEngine;

public class ProfileConfrontationsPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerConfrontations (this.id);
	}
}

