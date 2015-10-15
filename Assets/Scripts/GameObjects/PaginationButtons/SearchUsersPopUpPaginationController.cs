using UnityEngine;

public class SearchUsersPopUpPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		SearchUsersPopUpController.instance.paginationHandlerUsers (this.id);
	}
}

