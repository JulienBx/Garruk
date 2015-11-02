using UnityEngine;
using TMPro;

public class newMyGameSortButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.sortButtonHandler (base.getId());	
	}
}