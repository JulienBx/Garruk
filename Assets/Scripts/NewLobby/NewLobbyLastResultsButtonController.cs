using UnityEngine;
using TMPro;

public class NewLobbyLastResultsButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewLobbyController.instance.slideLeft ();
	}
}