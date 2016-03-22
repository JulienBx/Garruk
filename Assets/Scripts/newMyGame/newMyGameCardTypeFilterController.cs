using UnityEngine;
using TMPro;

public class newMyGameCardTypeFilterController : SpriteButtonController
{	
	new public void OnMouseUp()
	{
		if(!HelpController.instance.getIsTutorialLaunched())
		{
			newMyGameController.instance.cardTypeFilterHandler (base.getId());
		}
	}
}

