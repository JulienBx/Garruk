using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class HomePageTutorialController : TutorialObjectController 
{
	public static HomePageTutorialController instance;
	
	public override void startTutorial(int tutorialStep, bool isDisplayed)
	{
		if(tutorialStep==3 || tutorialStep ==4)
		{
			base.startTutorial(tutorialStep,true);
		}
		else
		{
			base.startTutorial(tutorialStep,isDisplayed);
		}
	}
	public override void endInitialization()
	{
		NewHomePageController.instance.endTutorialInitialization ();
	}
	public override void launchSequence(int sequenceID)
	{
		Vector3 gameObjectPosition = new Vector3 ();
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Bravo vous avez gagné !");
				this.setPopUpDescription("A compléter");
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 1: // Demande à l'utilisateur de créer un deck
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Dommage vous avez perdu !");
				this.setPopUpDescription("A compléter");
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
			}
			this.resizeBackground(new Rect(0,10,5,5),0f,0f);
			this.resizePopUp(new Vector3(0,0,-9.5f));
			break;
		case 2: // Demande à l'utilisateur de sélectionner des cartes
			if(!isResizing)
			{
				this.displayPopUp(0);
				this.setUpArrow();
				this.displayNextButton(true);
				this.setPopUpTitle("L'aide est toujours là");
				this.setPopUpDescription("A compléter");
				this.displayBackground(true);
				this.displayExitButton(false);
				this.displayDragHelp(false);
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
	
}

