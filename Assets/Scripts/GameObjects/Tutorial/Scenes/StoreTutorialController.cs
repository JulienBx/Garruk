using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class StoreTutorialController : TutorialObjectController 
{
	public static StoreTutorialController instance;
	
	public override void endInitialization()
	{
		NewStoreController.instance.endTutorialInitialization ();
	}
	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Bienvenue dans Techtical Wars");
				this.setPopUpDescription("A compléter");
				this.displayBackground(true);
				this.displayExitButton(false);
				
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1:
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setLeftArrow();
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				
			}
			gameObjectPosition = NewStoreController.instance.returnBuyPackButtonPosition(1);
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,ApplicationDesignRules.button62WorldSize.x+0.15f,ApplicationDesignRules.button62WorldSize.y+0.15f),1f,1f);
			this.drawLeftArrow();
			break;
		case 2:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
			}
			this.resizeBackground(new Rect(0,0,ApplicationDesignRules.worldWidth+1,4),0f,0f);
			break;
		case 3:
			if(this.getIsTutorialDisplayed())
			{
				if(!isResizing)
				{
					this.displayArrow(false);
					this.displayPopUp(0);
					this.displayNextButton(true);
					this.setPopUpTitle("Bravo voici vos premières recrues");
					this.setPopUpDescription("A compléter");
					this.displaySquareBackground(true);
					this.displayExitButton(true);
					
				}
				this.resizeBackground(new Rect(0,0,ApplicationDesignRules.worldWidth+1,4),0f,0f);
				this.resizePopUp(new Vector3(0,-3.5f,-9.5f));
			}
			else
			{
				this.sequenceID=100;
				goto default;
			}
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
		case 0: case 1: case 2: 
			this.launchSequence(this.sequenceID+1);
			break;
		case 3:
			this.sequenceID=100;
			this.launchSequence(this.sequenceID);
			break;
		}
	}
	public override int getStartSequenceId(int tutorialStep)
	{
		switch(tutorialStep)
		{
		case 1:
			return 0;
			break;
		default:
			return base.getStartSequenceId(tutorialStep);
			break;
		}
		return 0;
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
		case 0: // Présentation de l'écran de gestion des cartes
			if(NewStoreController.instance.getIsCardFocusedDisplayed())
			{
				this.sequenceID=100;
				goto default;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les packs");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=NewStoreController.instance.getPacksBlockOrigin();
			gameObjectPosition2=NewStoreController.instance.getStoreBlockOrigin();
			gameObjectSize=NewStoreController.instance.getPacksBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			break;
		case 1: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Acheter des crédits");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewStoreController.instance.getBuyCreditsBlockOrigin();
			gameObjectPosition2=NewStoreController.instance.getPacksBlockOrigin();
			gameObjectSize=NewStoreController.instance.getBuyCreditsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2: // Demande à l'utilisateur de sélectionner des cartes
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}
	
	public override GameObject getCardFocused()
	{
		return NewStoreController.instance.returnCardFocused ();
	}
	
	#endregion
}

