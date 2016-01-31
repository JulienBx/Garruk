using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class LobbyTutorialController : TutorialObjectController 
{
	public static LobbyTutorialController instance;
	
	public override void endInitialization()
	{
		NewLobbyController.instance.endTutorialInitialization ();
	}
	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		default:
			base.launchSequence(this.sequenceID);
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		default:
			base.actionIsDone();
			break;
		}
	}
	public override int getStartSequenceId(int tutorialStep)
	{
		switch(tutorialStep)
		{
		default:
			return base.getStartSequenceId(tutorialStep);
			break;
		}
		return 0;
	}
	public override void endHelp()
	{
		StartCoroutine(NewLobbyController.instance.endHelp ());
		if(ApplicationDesignRules.isMobileScreen)
		{
			if(NewLobbyController.instance.getAreResultsDisplayed())
			{
				NewLobbyController.instance.slideRight();
			}
			else if(NewLobbyController.instance.getAreStatsDisplayed())
			{
				NewLobbyController.instance.slideLeft();
			}
		}
		base.endHelp ();
	}
	#region HELP SEQUENCES
	
	public override void launchHelpSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector3 gameObjectPosition2 = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Encart de présentation de l'avancement dans la compétition "coupe ou division"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingLobbyTutorial.getHelpContent(0));
				this.setPopUpDescription(WordingLobbyTutorial.getHelpContent(1));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getMainBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				if(NewLobbyController.instance.getAreResultsDisplayed())
				{
					NewLobbyController.instance.slideRight();
				}
				else if(NewLobbyController.instance.getAreStatsDisplayed())
				{
					NewLobbyController.instance.slideLeft();
				}
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 1: // Encart d'affichage des derniers résutats au sein de la compétition
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingLobbyTutorial.getHelpContent(2));
				this.setPopUpDescription(WordingLobbyTutorial.getHelpContent(3));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getLastResultsBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewLobbyController.instance.slideLeft();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));	
			}
			break;
		case 2: // Encart de présentation de la compétition, récompenses, etc.
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingLobbyTutorial.getHelpContent(4));
				this.setPopUpDescription(WordingLobbyTutorial.getHelpContent(5));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewLobbyController.instance.getCompetitionBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getLastResultsBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getCompetitionBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewLobbyController.instance.slideRight();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.3f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,1f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 3: // Encart d'affichage des statistiques de l'utilisateur (sur l'ensemble des matchs classés)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingLobbyTutorial.getHelpContent(6));
				this.setPopUpDescription(WordingLobbyTutorial.getHelpContent(7));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewLobbyController.instance.getStatsBlockOrigin();
			gameObjectPosition2=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getStatsBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewLobbyController.instance.slideRight();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 4: 
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}
	
	#endregion

}

