using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		newMyGameController.instance.cardTypeFilterHandler (base.getId());	
	}
}

