using UnityEngine;
using TMPro;

public class NewProfileSearchButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.searchUsersHandler ();	
	}
}

