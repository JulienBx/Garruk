using UnityEngine;
using TMPro;

public class NewHomePageCompetitionController : SimpleButtonController 
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.joinGameHandler(base.getId());	
	}
}

