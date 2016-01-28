using UnityEngine;
using TMPro;

public class MobileMenuButtonController : SimpleButtonController
{
	public override void mainInstruction()
	{
		MenuController.instance.changePage (base.getId());	
	}
}

