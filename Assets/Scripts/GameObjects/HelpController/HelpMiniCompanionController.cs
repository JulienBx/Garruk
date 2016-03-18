using UnityEngine;
using TMPro;

public class HelpMiniCompanionController : SpriteButtonController
{
	public override void mainInstruction()
	{
		HelpController.instance.miniCompanionHandler();	
	}
}

