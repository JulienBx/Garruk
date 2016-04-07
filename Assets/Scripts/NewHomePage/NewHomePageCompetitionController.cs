using UnityEngine;
using TMPro;

public class NewHomePageCompetitionController : SimpleButtonController 
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.joinGameHandler(base.getId());	
	}
	public override void showToolTip ()
	{
		if(base.getId()==0)
		{
			BackOfficeController.instance.displayToolTip(WordingGameModes.getReference(13),WordingGameModes.getReference(14));
		}
		else if(ApplicationModel.player.TrainingStatus!=-1)
		{
			BackOfficeController.instance.displayToolTip(WordingGameModes.getReference(15),WordingGameModes.getReference(16));
		}
		else
		{
			BackOfficeController.instance.displayToolTip(WordingGameModes.getReference(17),WordingGameModes.getReference(18));
		}
	}
}

