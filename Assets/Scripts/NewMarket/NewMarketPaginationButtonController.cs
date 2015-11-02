using UnityEngine;
using TMPro;

public class NewMarketPaginationButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewMarketController.instance.paginationHandler (base.getId());	
	}
}

