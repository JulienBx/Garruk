using UnityEngine;
using TMPro;

public class MenuButtonController : SimpleButtonController
{
	public override void mainInstruction()
	{
		MenuController.instance.changePage (base.getId());	
	}
}

