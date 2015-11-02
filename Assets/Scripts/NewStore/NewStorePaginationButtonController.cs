using UnityEngine;
using TMPro;

public class NewStorePaginationButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewStoreController.instance.paginationHandler (base.getId());	
	}
}

