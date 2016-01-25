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
	public override void endHelp()
	{
		StartCoroutine(NewMarketController.instance.endHelp ());
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
		case 0: // Encart des cartes (trois onglets, en vente, mes ventes, disponibles)
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
				this.setPopUpTitle("Recruter sur le marché");
				this.setPopUpDescription("Les colons peuvent acquérir des unités appartenant à leurs confrères, moyennant une compensation décidée par le vendeur. Les unités disponibles sur ce marché sont toutes visibles ici");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			
			gameObjectPosition=NewMarketController.instance.getCardsBlockOrigin();
			gameObjectPosition2=NewMarketController.instance.getMarketBlockOrigin();
			gameObjectSize=NewMarketController.instance.getCardsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			break;
		case 1: // Encart de présentation des filtres
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Se repérer sur le marché");
				this.setPopUpDescription("Le marché est immense, mais ces filtres vous permettront de mettre la main sur l'unité idéale pour vos équipes !");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewMarketController.instance.getFiltersBlockOrigin();
			gameObjectPosition2=NewMarketController.instance.getCardsBlockOrigin();
			gameObjectSize=NewMarketController.instance.getFiltersBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2:
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

