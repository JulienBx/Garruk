using UnityEngine;
using TMPro;

public class AuthenticationQuitButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		Application.Quit();
	}
}
