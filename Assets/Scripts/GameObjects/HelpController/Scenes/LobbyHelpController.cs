using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyHelpController : HelpController 
{

	#region Help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewLobbyController.instance.returnMainBlock (),true);
			this.setCompanion (WordingLobbyHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewLobbyController.instance.returnStatsBlock (),true);
			this.setCompanion(WordingLobbyHelp.getHelpContent(3),true,false,false,0f);
			break;
		case 2:
			this.setCompanion (WordingLobbyHelp.getHelpContent (1), true, true, true, 0f);
			this.setFlashingBlock (NewLobbyController.instance.returnLastResultsBlock (),true);
			break;
		case 3:
			this.setCompanion(WordingLobbyHelp.getHelpContent(2),true,true,false,0f);
			this.setFlashingBlock (NewLobbyController.instance.returnCompetitionBlock (),true);
			break;
		}
	}
	public override void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewLobbyController.instance.returnMainBlock (),true);
			if (NewLobbyController.instance.getAreResultsDisplayed ()) 
			{
				NewLobbyController.instance.slideRight ();
			} 
			else if (NewLobbyController.instance.getAreStatsDisplayed ()) 
			{
				NewLobbyController.instance.slideLeft ();
			}
			this.setCompanion (WordingLobbyHelp.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewLobbyController.instance.returnStatsBlock (),true);
			NewLobbyController.instance.slideRight ();
			this.setCompanion(WordingLobbyHelp.getHelpContent(3),true,true,false,0f);
			break;
		case 2:
			NewLobbyController.instance.slideLeft ();
			this.setCompanion (WordingLobbyHelp.getHelpContent (2), true, false, false, 4f);
			this.setFlashingBlock (NewLobbyController.instance.returnCompetitionBlock (),true);	
			break;
		case 3:
			NewLobbyController.instance.slideLeft ();
			this.setCompanion(WordingLobbyHelp.getHelpContent(1),true,false,false,0f);
			this.setFlashingBlock (NewLobbyController.instance.returnLastResultsBlock (),true);
			break;
		}
	}
	public override void getHelpNextAction()
	{
		if (sequenceId < 3) 
		{
			this.sequenceId++;
			this.launchHelpSequence ();
		} 
		else 
		{
			StartCoroutine (NewLobbyController.instance.endHelp ());
			this.quitHelp ();
		}
	}

	#endregion
}

