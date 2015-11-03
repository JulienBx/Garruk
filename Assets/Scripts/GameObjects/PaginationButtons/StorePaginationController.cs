using UnityEngine;

public class StorePaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewStoreController.instance.paginationHandler ();
	}
}

