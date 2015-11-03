using UnityEngine;
using TMPro;

public class newMyGamePaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		newMyGameController.instance.paginationHandler ();
	}
}

