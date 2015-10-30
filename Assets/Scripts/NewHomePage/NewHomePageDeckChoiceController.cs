using UnityEngine;
using TMPro;

public class NewHomePageDeckChoiceController : SimpleButtonController 
{
	public override void OnMouseDown()
	{
		NewHomePageController.instance.selectDeck (base.getId());	
	}
}

