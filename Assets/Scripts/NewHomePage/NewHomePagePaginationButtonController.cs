using UnityEngine;
using TMPro;

public class NewHomePagePaginationButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.paginationHandler (base.getId());	
	}
}

