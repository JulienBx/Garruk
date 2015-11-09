using UnityEngine;
using TMPro;

public class SearchUsersPopUpPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		gameObject.transform.parent.GetComponent<SearchUsersPopUpController>().paginationHandler();
	}
}

