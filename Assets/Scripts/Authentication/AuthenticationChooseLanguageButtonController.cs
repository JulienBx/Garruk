using UnityEngine;
using TMPro;

public class AuthenticationChooseLanguageButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		AuthenticationController.instance.chooseLanguageHandler();
	}
}

