using UnityEngine;
using TMPro;

public class newMyGameDeckRenameButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.displayEditDeckPopUp ();
	}
}

