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
		case 1: // Demande à l'utilisateur de créer un deck
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setRightArrow();
				this.displayNextButton(false);
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
				this.displayExitButton(true);
				
			}
			gameObjectPosition = newMyGameController.instance.getNewDeckButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,2.5f,1f),0.8f,0.8f);
			this.drawRightArrow();
			break;
		case 2: // Demande à l'utilisateur de sélectionner des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayDragHelp(true);
				this.displayPopUp(0);
				this.displayNextButton(false);
				this.displayBackground(true);
				this.displayExitButton(false);
				this.setPopUpTitle("Déplacer les cartes");
				this.setPopUpDescription("A compléter");
				this.displayExitButton(true);
			}
			this.resizeBackground(new Rect(0,0,ApplicationDesignRules.worldWidth+6,7),1f,0.4f);
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
	
}

