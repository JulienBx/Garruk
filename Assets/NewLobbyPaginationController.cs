using UnityEngine;
using TMPro;

public class NewLobbyPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewLobbyController.instance.paginationHandler ();
	}
}

