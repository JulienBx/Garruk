using UnityEngine;
using TMPro;

public class newMyGamePaginationButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.paginationHandler (base.getId());	
	}
}

