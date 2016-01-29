using UnityEngine;
using TMPro;

public class newMyGameSortButtonController : SpriteButtonController
{	
	public void OnMouseUp()
	{
		newMyGameController.instance.sortButtonHandler (base.getId());	
	}
}