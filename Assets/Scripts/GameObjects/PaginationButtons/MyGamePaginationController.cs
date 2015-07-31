using UnityEngine;

public class MyGamePaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		newMyGameController.instance.paginationHandler (this.id);
	}
}

