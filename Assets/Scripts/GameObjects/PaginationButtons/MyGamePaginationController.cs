using UnityEngine;

public class MyGamePaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		//newMyGameController.instance.paginationHandler (this.id);
	}
}

