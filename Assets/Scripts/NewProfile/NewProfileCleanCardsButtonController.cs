using UnityEngine;
using TMPro;

public class NewProfileCleanCardsButtonController : TextButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.cleanCardsHandler ();
	}
}

