using UnityEngine;
using TMPro;

public class NewSkillBookHelpPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewSkillBookController.instance.paginationHelpHandler();
	}
}

