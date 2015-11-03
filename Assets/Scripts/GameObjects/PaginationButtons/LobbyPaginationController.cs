using UnityEngine;

public class LobbyPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewLobbyController.instance.paginationHandler (this.id);
	}
}

