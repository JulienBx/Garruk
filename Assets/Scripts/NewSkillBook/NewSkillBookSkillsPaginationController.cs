using UnityEngine;
using TMPro;

public class NewSkillBookSkillsPaginationController : PaginationController
{	
	public override void paginationHandler(int id)
	{
		base.paginationHandler (id);
		NewSkillBookController.instance.paginationSkillHandler ();
	}
}

