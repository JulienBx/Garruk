using UnityEngine;

public class SkillBookPaginationController : OldPaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewSkillBookController.instance.paginationHandler (this.id);
	}
}

