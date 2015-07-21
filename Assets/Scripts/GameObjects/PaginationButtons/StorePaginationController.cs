using UnityEngine;

public class StorePaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewStoreController.instance.paginationHandler (this.id);
	}
}

