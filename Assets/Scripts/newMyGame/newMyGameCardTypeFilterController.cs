using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.cardTypeFilterHandler (base.getId());
		}
	}
}

