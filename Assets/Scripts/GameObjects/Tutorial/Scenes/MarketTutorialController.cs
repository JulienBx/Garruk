using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MarketTutorialController : TutorialObjectController 
{
	public static MarketTutorialController instance;
	
	public override void endInitialization()
	{
		NewMarketController.instance.endTutorialInitialization ();
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
			if(NewMarketController.instance.getIsCardFocusedDisplayed())
			{
				this.sequenceID=100;
				goto default;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les cartes");
				this.setPopUpDescription("A compléter");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=NewMarketController.instance.getCardsBlockOrigin();
			gameObjectPosition2=NewMarketController.instance.getMarketBlockOrigin();
			gameObjectSize=NewMarketController.instance.getCardsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 1: // Présentation de l'écran de gestion des cartes
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
			gameObjectPosition=NewMarketController.instance.getFiltersBlockOrigin();
			gameObjectPosition2=NewMarketController.instance.getCardsBlockOrigin();
			gameObjectSize=NewMarketController.instance.getFiltersBlockSize();
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
		return NewMarketController.instance.returnCardFocused ();
	}
	
	#endregion
}

