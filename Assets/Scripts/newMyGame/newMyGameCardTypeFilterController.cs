using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		newMyGameController.instance.cardTypeFilterHandler (base.getId());	
	}
}

