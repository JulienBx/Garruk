using UnityEngine;
using TMPro;

public class NewProfileCollectionButtonController : TextButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.collectionButtonHandler ();
	}
}

