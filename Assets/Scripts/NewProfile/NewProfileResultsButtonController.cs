using UnityEngine;
using TMPro;

public class NewProfileResultsButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.slideRight ();
	}
}