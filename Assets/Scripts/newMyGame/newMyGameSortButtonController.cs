using UnityEngine;
using TMPro;

public class newMyGameSortButtonController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.sortButtonHandler (base.getId());
		}
	}
}