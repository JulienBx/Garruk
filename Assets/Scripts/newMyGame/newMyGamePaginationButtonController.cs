using UnityEngine;
using TMPro;

public class newMyGamePaginationButtonController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.paginationHandler (base.getId());	
	}
}

