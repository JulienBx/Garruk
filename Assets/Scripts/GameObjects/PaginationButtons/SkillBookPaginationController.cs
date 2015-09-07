using UnityEngine;

public class SkillBookPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewSkillBookController.instance.paginationHandler (this.id);
	}
}

