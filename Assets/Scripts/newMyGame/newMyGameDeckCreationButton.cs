using UnityEngine;
using TMPro;

public class newMyGameDeckCreatioButtonController : SimpleButtonController
{	
	public override void OnMouseDown()
	{
		newMyGameController.instance.displayNewDeckPopUp();
	}
}

