using UnityEngine;
using TMPro;

public class newMyGamePaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			base.paginationHandler (id);
			newMyGameController.instance.paginationHandler ();
		}
	}
}

