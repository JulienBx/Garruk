using UnityEngine;
using TMPro;

public class NewMarketPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewMarketController.instance.paginationHandler ();
	}
}
