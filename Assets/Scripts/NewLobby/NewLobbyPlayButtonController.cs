using UnityEngine;
using TMPro;

public class NewLobbyPlayButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewLobbyController.instance.playHandler ();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingLobby.getReference(39),WordingLobby.getReference(40));
	}
}

