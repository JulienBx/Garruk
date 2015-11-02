using UnityEngine;
using TMPro;

public class NewHomePageDeckChoiceController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewHomePageController.instance.selectDeck (base.getId());	
	}
}

