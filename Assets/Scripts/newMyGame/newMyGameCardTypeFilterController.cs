using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.cardTypeFilterHandler (base.getId());	
	}
}

