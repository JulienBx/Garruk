using UnityEngine;
using TMPro;

public class newMyGameDeckChoiceController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		newMyGameController.instance.selectDeck (base.getId());	
	}
	public void OnMouseDown()
	{
		base.OnMouseUp ();
	}
}

