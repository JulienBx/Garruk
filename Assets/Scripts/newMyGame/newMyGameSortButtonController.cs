using UnityEngine;
using TMPro;

public class newMyGameSortButtonController : SpriteButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.sortButtonHandler (base.getId());	
	}
}