using UnityEngine;
using TMPro;

public class NewStorePaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		if (HelpController.instance.canAccess (-1)) {
			base.paginationHandler (id);
			NewStoreController.instance.paginationHandler ();
		}
	}
}

