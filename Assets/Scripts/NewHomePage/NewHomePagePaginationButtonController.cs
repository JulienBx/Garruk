using UnityEngine;
using TMPro;

public class NewHomePagePaginationButtonController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		NewHomePageController.instance.paginationHandler (base.getId());	
	}
}

