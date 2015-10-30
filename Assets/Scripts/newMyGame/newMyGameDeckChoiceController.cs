using UnityEngine;
using TMPro;

public class newMyGameDeckChoiceController : SimpleButtonController 
{
	public override void OnMouseDown()
	{
		newMyGameController.instance.selectDeck (base.getId());	
	}
}

