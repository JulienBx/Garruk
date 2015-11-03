using UnityEngine;
using TMPro;

public class NewHomePagePaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewHomePageController.instance.paginationHandler ();
	}
}

