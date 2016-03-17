using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyHelpController : HelpController 
{
	public override void getSequenceSettings()
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();

		switch (this.sequenceId) 
		{
		case 0:
			
			this.setFlashingBlock (NewLobbyController.instance.returnMainBlock ());
			if (ApplicationDesignRules.isMobileScreen) 
			{
				if (NewLobbyController.instance.getAreResultsDisplayed ()) 
				{
					NewLobbyController.instance.slideRight ();
				} 
				else if (NewLobbyController.instance.getAreStatsDisplayed ()) 
				{
					NewLobbyController.instance.slideLeft ();
				}
				this.setCompanion (WordingLobbyTutorial.getHelpContent (1), true, true, true, 0f);
			} 
			else 
			{
				this.setCompanion (WordingLobbyTutorial.getHelpContent (1), true, false, true, 0f);
			}
			break;
		case 1:
			this.setFlashingBlock (NewLobbyController.instance.returnStatsBlock ());
			if (ApplicationDesignRules.isMobileScreen) 
			{
				NewLobbyController.instance.slideRight ();
				this.setCompanion(WordingLobbyTutorial.getHelpContent(7),true,true,false,0f);
			} 
			else 
			{
				this.setCompanion(WordingLobbyTutorial.getHelpContent(7),true,false,false,0f);
			}
			break;
		case 2:
			if (ApplicationDesignRules.isMobileScreen) 
			{
				NewLobbyController.instance.slideLeft ();
				this.setCompanion (WordingLobbyTutorial.getHelpContent (5), true, false, false, 4f);
				this.setFlashingBlock (NewLobbyController.instance.returnCompetitionBlock ());
			} 
			else 
			{
				this.setCompanion (WordingLobbyTutorial.getHelpContent (3), true, true, true, 0f);
				this.setFlashingBlock (NewLobbyController.instance.returnLastResultsBlock ());
			}	
			break;
		case 3:
			if (ApplicationDesignRules.isMobileScreen) 
			{
				NewLobbyController.instance.slideLeft ();
				this.setCompanion(WordingLobbyTutorial.getHelpContent(3),true,false,false,0f);
				this.setFlashingBlock (NewLobbyController.instance.returnLastResultsBlock ());
			} 
			else 
			{
				this.setCompanion(WordingLobbyTutorial.getHelpContent(5),true,true,false,0f);
				this.setFlashingBlock (NewLobbyController.instance.returnCompetitionBlock ());
			}
			break;
		}
	}
	public override void companionNextButtonHandler()
	{
		switch (this.sequenceId) 
		{
		case 0: case 1: case 2:
			this.sequenceId++;
			this.launchSequence();
			break;
		case 3:
			this.quitHelp ();
			break;
		}
	}

}

