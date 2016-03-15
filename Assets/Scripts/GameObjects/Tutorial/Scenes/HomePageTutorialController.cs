using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageTutorialController : TutorialObjectController 
{
	new public static HomePageTutorialController instance;
	
	public override void startTutorial()
	{
		if(ApplicationModel.player.TutorialStep==3 || ApplicationModel.player.TutorialStep ==4)
		{
			ApplicationModel.player.DisplayTutorial=true;
			base.startTutorial();
		}
		else
		{
			base.startTutorial();
		}
	}
	#region TUTORIAL SEQUENCES

	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Texte de clôture du tutorial en cas de victoire
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getTutorialContent(0));
				this.setPopUpDescription(WordingHomePageTutorial.getTutorialContent(1));
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false,false);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1: // Texte de clôture du tutorial en cas de défaite
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getTutorialContent(2));
				this.setPopUpDescription(WordingHomePageTutorial.getTutorialContent(3));
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false,false);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 2: // Texte pour expliquer que l'aide est toujours disponible
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getTutorialContent(4));
				this.setPopUpDescription(WordingHomePageTutorial.getTutorialContent(5));
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition = MenuController.instance.getHelpButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.5f,1.5f),0f,0f);
			this.drawUpArrow();
			break;
		default:
			base.launchSequence(this.sequenceID);
			break;
		}
	}
	public override void actionIsDone()
	{
		switch(this.sequenceID)
		{
		case 0: case 1:
			this.sequenceID=2;
			launchSequence(this.sequenceID);
			break;
		case 2:
			StartCoroutine(this.endTutorial());
			break;
		default:
			base.actionIsDone();
			break;
		}
	}
	public override int getStartSequenceId(int tutorialStep)
	{
		switch(tutorialStep)
		{
		case 3:
			return 0;
			break;
		case 4:
			return 1;
			break;
		default:
			return base.getStartSequenceId(tutorialStep);
			break;
		}
		return 0;
	}

	#endregion

	#region HELP SEQUENCES

	public override void launchHelpSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector3 gameObjectPosition2 = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Encart "jouer"
			if(NewHomePageController.instance.getIsCardFocusedDisplayed())
			{
				this.sequenceID=100;
				goto default;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getHelpContent(0));
				this.setPopUpDescription(WordingHomePageTutorial.getHelpContent(1));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getPlayBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getPlayBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				if(!NewHomePageController.instance.getIsMainContentDisplayed())
				{
					NewHomePageController.instance.slideRight();	
				}
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-2f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 1: // Encart "mon équipe"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getHelpContent(2));
				this.setPopUpDescription(WordingHomePageTutorial.getHelpContent(3));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getNewsfeedBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getDeckBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.3f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,2f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));	
			}

			break;
		case 2: // Encart "boutique"
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.sequenceID=3;
				goto case 3;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getHelpContent(4));
				this.setPopUpDescription(WordingHomePageTutorial.getHelpContent(5));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getStoreBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getNewsfeedBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getStoreBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 3: // Encart "Réseau social"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingHomePageTutorial.getHelpContent(6));
				this.setPopUpDescription(WordingHomePageTutorial.getHelpContent(7));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getNewsfeedBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getNewsfeedBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewHomePageController.instance.slideLeft();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3f,-9.5f));	
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

	public override GameObject getCardFocused()
	{
		return NewHomePageController.instance.returnCardFocused();
	}
	public override void endHelp()
	{
		if(ApplicationDesignRules.isMobileScreen && !NewHomePageController.instance.getIsMainContentDisplayed())
		{
			NewHomePageController.instance.slideRight();
		}
		base.endHelp();
	}
	#endregion
}

