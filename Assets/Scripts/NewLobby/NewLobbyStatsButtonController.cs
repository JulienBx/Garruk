using UnityEngine;
using TMPro;

public class NewLobbyStatsButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewLobbyController.instance.slideRight ();
	}
}