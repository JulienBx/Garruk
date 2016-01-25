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

	#region TUTORIAL SEQUENCES

	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Arrivée dans le store après le premier écran d'introduction
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Centre de recrutement");
				this.setPopUpDescription("Dès leur arrivée sur Cristalia, les colons sont emmenés au centre de recrutement. Mieux vaut en effet ne pas se promener seul sur la planète... Votre pays vous a remis une somme d'argent que vous avez pu échanger contre la monnaie locale (le Cristal). Ceci devrait vous permettre de recruter quelques Cristaliens prets à se battre pour vous");
				this.displayBackground(true);
				this.displayExitButton(false);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0f,0f,-9.5f));
			break;
		case 1: // On achète un pack (pas de texte)
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.displayNextButton(false);
				this.displayBackground(true);
				this.displayExitButton(false);
			}
			this.setIsScrolling(true);
			if(!getIsScrolling())
			{
				this.displayScrollDownHelp(false);
				this.displayScrollUpHelp(false);
				this.setLeftArrow();
				gameObjectPosition = NewStoreController.instance.returnBuyPackButtonPosition(1);
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,ApplicationDesignRules.button62WorldSize.x+1f,ApplicationDesignRules.button62WorldSize.y+0.15f),1f,1f);
				this.drawLeftArrow();
			}
			break;
		case 2: // Achat du pack (pas de texte)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(-1);
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				this.setIsScrolling(false);
			}
			if(ApplicationDesignRules.isMobileScreen)
			{
				this.resizeBackground(new Rect(0,1f,ApplicationDesignRules.worldWidth+1,6f),0f,0f);
			}
			else
			{
				this.resizeBackground(new Rect(0,-0.25f,ApplicationDesignRules.worldWidth+1,5.5f),0f,0f);
			}
			break;
		case 3: // Affichage des premières recrues
			if(this.getIsTutorialDisplayed())
			{
				if(!isResizing)
				{
					this.displayArrow(false);
					this.displayPopUp(0);
					this.displayNextButton(true);
					this.setPopUpTitle("Vos premières unités");
					this.setPopUpDescription("Consultez-les attentivement pour comprendre leurs forces et faiblesses. Il est maintenant temps d'organiser vos troupes pour disputer votre premier combat");
					this.displaySquareBackground(true);
					this.displayExitButton(true);
					this.setIsScrolling(false);
				}
				if(ApplicationDesignRules.isMobileScreen)
				{
					this.resizeBackground(new Rect(0,1f,ApplicationDesignRules.worldWidth+1,6f),0f,0f);
					this.resizePopUp(new Vector3(0,-3.5f,-9.5f));
				}
				else
				{
					this.resizeBackground(new Rect(0,-0.25f,ApplicationDesignRules.worldWidth+1,5.5f),0f,0f);
					this.resizePopUp(new Vector3(0,-3.5f,-9.5f));
				}

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
	public override void scrollingExceptions()
	{
		Vector3 gameObjectPosition = new Vector3 ();
		switch(this.sequenceID)
		{
		case 1:
			float mediumScrollCameraPositionY=NewStoreController.instance.getMediumScrollCameraPosition().y;
			if(mediumScrollCameraPositionY>0.64f)
			{
				this.displayScrollDownHelp(true);
				this.displayScrollUpHelp(false);
				this.displayArrow(false);
				this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			}
			else if(mediumScrollCameraPositionY>-2.95f)
			{
				gameObjectPosition = NewStoreController.instance.returnBuyPackButtonPosition(1);
				float yPosition = gameObjectPosition.y-mediumScrollCameraPositionY+ApplicationDesignRules.topBarWorldSize.y/2f;
				if(!this.getIsMoving())
				{
					this.displayScrollDownHelp(false);
					this.displayScrollUpHelp(false);
					this.setLeftArrow();
					this.resizeBackground(new Rect(gameObjectPosition.x,yPosition,ApplicationDesignRules.button62WorldSize.x+1f,ApplicationDesignRules.button62WorldSize.y+0.15f),1f,1f);
					this.drawLeftArrow();
				}
				this.adjustLeftArrowY(yPosition);
				this.adjustBackgroundY(yPosition);
			}
			else
			{
				this.displayScrollUpHelp(true);
				this.displayScrollDownHelp(false);
				this.displayArrow(false);
				this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			}
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
		case 0: // Encart les packs
			if(NewStoreController.instance.getIsCardFocusedDisplayed())
			{
				this.sequenceID=100;
				goto default;
			}
			else if(NewStoreController.instance.getAreRandomCardsDisplayed())
			{
				this.sequenceID=3;
				goto case 3;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les groupes");
				this.setPopUpDescription("Certains cristaliens s'entrainent ensemble depuis leur enfance et peuvent etre recrutés à des tarifs intéressants");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			
			gameObjectPosition=NewStoreController.instance.getPacksBlockOrigin();
			gameObjectPosition2=NewStoreController.instance.getStoreBlockOrigin();
			gameObjectSize=NewStoreController.instance.getPacksBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			break;
		case 1: // Encart acheter des crédits
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Acheter du Cristal");
				this.setPopUpDescription("Pour investir sur de nouvelles unités et progresser plus rapidement");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewStoreController.instance.getBuyCreditsBlockOrigin();
			gameObjectPosition2=NewStoreController.instance.getPacksBlockOrigin();
			gameObjectSize=NewStoreController.instance.getBuyCreditsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2: 
			this.endHelp();
			break;
		case 3: // Aide qui s'affiche lorsque le joueur à acheter plusieurs cartes (on peut cliquer sur une carte pour avoir le détail)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(2);
				this.displayNextButton(true);
				this.setPopUpTitle("Les unités");
				this.setPopUpDescription("Voici les unités que vous avez acquises au centre de recrutement. Elles sont directement transférées vers votre armée");
				this.displayBackground(true);
				this.displayExitButton(false);
				
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
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
		return NewStoreController.instance.returnCardFocused ();
	}
	
	#endregion
}

