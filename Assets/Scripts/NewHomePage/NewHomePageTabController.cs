using UnityEngine;
using TMPro;

public class NewHomePageTabController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.selectATabHandler(base.getId());	
	}
}

