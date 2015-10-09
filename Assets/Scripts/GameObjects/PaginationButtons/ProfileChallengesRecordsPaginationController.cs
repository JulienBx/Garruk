using UnityEngine;

public class ProfileChallengesRecordsPaginationController : PaginationController
{
	void OnMouseDown()
	{
		this.isActive=!this.isActive;
		base.setSprite ();
		NewProfileController.instance.paginationHandlerChallengesRecords (this.id);
	}
}

