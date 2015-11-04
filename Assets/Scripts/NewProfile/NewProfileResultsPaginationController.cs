using UnityEngine;
using TMPro;

public class NewProfileResultsPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewProfileController.instance.paginationResultsHandler ();
	}
}

