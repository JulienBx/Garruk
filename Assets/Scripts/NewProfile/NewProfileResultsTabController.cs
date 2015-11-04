using UnityEngine;
using TMPro;

public class NewProfileResultsTabController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.selectAResultsTabHandler(base.getId());	
	}
}

