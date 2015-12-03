using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameTutorialController : TutorialObjectController 
{
	public static MyGameTutorialController instance;
	
	public override void endInitialization()
	{
		newMyGameController.instance.endTutorialInitialization ();
	}

	#region TUTORIAL SEQUENCES

	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Présentation de l'écran de gestion des cartes
			if(this.getIsTutorialDisplayed())
			{
				if(!isResizing)
				{
					this.displayArrow(false);
					this.displayPopUp(2);
					this.displayNextButton(true);
					this.setPopUpTitle("Bienvenue sur l'écran de gestion des cartes");
					this.setPopUpDescription("A compléter");
					this.displayBackground(true);
					this.displayExitButton(false);
					this.displayDragHelp(false);
					this.displayExitButton(true);
					
				}
				this.resizeBackground(new Rect(0,10,5,5),0f,0f);
				this.resizePopUp(new Vector3(0,0,-9.5f));
			}
			else
			{
				this.sequenceID=1;
				goto case 1;
			}
			break;
		case 1: // Demande à l'utilisateur de créer un deck (pas de texte)
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setRightArrow();
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
				this.displayExitButton(true);
				
			}
			gameObjectPosition = newMyGameController.instance.getNewDeckButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,ApplicationDesignRules.button61WorldSize.x+0.3f,ApplicationDesignRules.button61WorldSize.y+0.3f),1f,1f);
			this.drawRightArrow();
			break;
		case 2: // Demande à l'utilisateur de sélectionner des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayDragHelp(true);
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				this.setPopUpTitle("Déplacer les cartes");
				this.setPopUpDescription("A compléter");
				this.displayExitButton(true);
			}
			this.resizeBackground(new Rect(0,-1.25f,ApplicationDesignRules.worldWidth+1,6),1f,0.9f);
			this.resizeDragHelp(new Vector3(0f,0f,0f));
			this.resizePopUp(new Vector3(0,-3f,-9.5f));
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
			this.launchSequence(this.sequenceID+1);
			break;
		case 2:
			if(ApplicationModel.hasDeck)
			{
				this.sequenceID=101;
			}
			else if(!newMyGameController.instance.isADeckCurrentlySelected())
			{
				this.sequenceID=0;
			}
			else
			{
				this.sequenceID=2;
			}
			this.launchSequence(this.sequenceID);
			break;
		case 101:
			if(MenuController.instance.getIsPlayPopUpDisplayed())
			{
				this.sequenceID=102;
				this.launchSequence(this.sequenceID);
			}
			else
			{
				goto case 2;
			}
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
		case 2:
			if(!ApplicationModel.hasDeck)
			{
				if(newMyGameController.instance.isADeckCurrentlySelected())
				{
					return 2;
				}
				else
				{
					return 0;
				}
			}
			else
			{
				goto default;
			}
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
		case 0: // Encart mes cartes
			if(newMyGameController.instance.getIsCardFocusedDisplayed())
			{
				this.sequenceID=100;
				goto default;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mes cartes");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=newMyGameController.instance.getCardsBlockOrigin();
			gameObjectPosition2=newMyGameController.instance.getDeckBlockOrigin();
			gameObjectSize=newMyGameController.instance.getCardsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			break;
		case 1: // Encart mon équipe
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mon équipe");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=newMyGameController.instance.getDeckBlockOrigin();
			gameObjectPosition2=newMyGameController.instance.getCardsBlockOrigin();
			gameObjectSize=newMyGameController.instance.getDeckBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2: // Encart les filtres
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les filtres");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=newMyGameController.instance.getFiltersBlockOrigin();
			gameObjectPosition2=newMyGameController.instance.getCardsBlockOrigin();
			gameObjectSize=newMyGameController.instance.getFiltersBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 3: 
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}
	
	public override GameObject getCardFocused()
	{
		return newMyGameController.instance.returnCardFocused ();
	}
	
	#endregion
}

