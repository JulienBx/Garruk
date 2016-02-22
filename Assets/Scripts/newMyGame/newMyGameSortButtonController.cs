using UnityEngine;
using TMPro;

public class newMyGameSortButtonController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		if(!TutorialObjectController.instance.getIsTutorialDisplayed())
		{
			newMyGameController.instance.sortButtonHandler (base.getId());
		}
	}
}