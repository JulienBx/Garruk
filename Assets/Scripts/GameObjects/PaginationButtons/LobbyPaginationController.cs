using UnityEngine;

public class LobbyPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewLobbyController.instance.paginationHandler (this.id);
	}
}

