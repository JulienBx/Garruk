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
					this.setPopUpTitle("Mon armée");
					this.setPopUpDescription("La meilleure manière de s'enrichir à Cristalia est de combattre d'autres colons ! Vous constituer une armée puissante sera donc cruciale pour survivre sur la planète. Vous pouvez ici consulter vos unités, et les organiser en équipes pretes à combattre pour acquérir de nouvelles ressources");
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
				this.setPopUpTitle("Créer une équipe");
				this.setPopUpDescription("A cause de massacres fréquents menaçant la survie de l'espèce humaine sur Cristalia, les combats sont depuis peu règlementés et opposent des équipes de 4 unités. Créer votre équipe en choisissant vos meilleures unités!");
				this.displayExitButton(true);
			}
			this.resizeBackground(new Rect(0,-1.25f,ApplicationDesignRules.worldWidth+1,6),1f,0.9f);
			this.resizeDragHelp(new Vector3(0f,0f,0f));
			this.resizePopUp(new Vector3(0,-3f,-9.5f));
			break;
		case 3: 
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setDownArrow();
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
				this.displayExitButton(true);
			}
			gameObjectPosition = this.getCardFocused().transform.FindChild("FocusFeature5").position;
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,1.6f*ApplicationDesignRules.button62WorldSize.x+0.3f,1.5f*ApplicationDesignRules.button62WorldSize.y+0.3f),1f,1f);
			this.drawDownArrow();
			break;
		case 4: 
			if(!isResizing)
			{
				this.displayPopUp(-1);
				this.setDownArrow();
				this.displayNextButton(false);
				this.displaySquareBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
				this.displayExitButton(true);
			}
			gameObjectPosition = PlayPopUpController.instance.getQuitPopUpButtonPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,3.5f,1f),0.8f,0.8f);
			this.drawDownArrow();
			break;
		default:
			base.launchSequence(this.sequenceID);
			break;
		}
	}
	public override void actionIsDone()
	{
		if(MenuController.instance.getIsPlayPopUpDisplayed())
		{
			if(ApplicationModel.hasDeck)
			{
				this.sequenceID=102;
			}
			else
			{
				this.sequenceID=4;
			}
		}
		else if(newMyGameController.instance.getIsFocusedCardDisplayed())
		{
			this.sequenceID=3;
		}
		else if(ApplicationModel.hasDeck)
		{
			this.sequenceID=101;
		}
		else if(newMyGameController.instance.isADeckCurrentlySelected())
		{
			this.sequenceID=2;
		}
		else
		{
			this.sequenceID=0;
		}
		this.launchSequence (this.sequenceID);
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
				this.setPopUpTitle("Mes unités");
				this.setPopUpDescription("Accédez à l'ensemble de vos unités, et n'hésitez pas à cliquer sur une unité pour accéder au détail de ses compétences");
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
				this.setPopUpTitle("Mes équipes");
				this.setPopUpDescription("Organisez vos unités en équipes de 4 pretes à combattre. N'oubliez jamais que l'ordre des unités dans l'équipe se retrouve également dans l'ordre de jeu en combat.");
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
				this.setPopUpDescription("Plus vous posséderez d'unités, plus il sera difficile de bien toutes les connaitre. Les filtres vous permettront de trouver rapidement des unités répondant à des critères spécifiques");
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

