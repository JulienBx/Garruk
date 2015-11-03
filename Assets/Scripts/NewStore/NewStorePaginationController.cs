using UnityEngine;
using TMPro;

public class NewStorePaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewStoreController.instance.paginationHandler ();
	}
}

