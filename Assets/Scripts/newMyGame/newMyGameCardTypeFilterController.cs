using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.cardTypeFilterHandler (base.getId());
		}
	}
}

