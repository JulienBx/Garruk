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
				this.setPopUpTitle("Bravo !");
				this.setPopUpDescription("Vous avez gagné votre premier combat. Défiez de nouveaux colons pour tester l'étendue de vos capacités, et continuez à améliorer votre équipe pour pouvoir triompher de tous types d'ennemis");
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
				this.setPopUpTitle("Défaite !");
				this.setPopUpDescription("Vos unités se sont bien débrouillées pour leur premier combat, mais l'adversaire était trop fort ! Continuez à entrainer vos troupes pour les préparer aux combats qui les attendent !");
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
				this.setPopUpTitle("Aidez-moi !");
				this.setPopUpDescription("Quelque soit l'endroit ou vous vous trouvez sur Cristalia, je reste à votre disposition si vous avez une question ou que vous vous sentez perdus ! Cliquez sur l'aide pour me convoquer");
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
		case 0: // Encart "mon équipe"
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
				this.setPopUpTitle("Mon équipe");
				this.setPopUpDescription("Les combats de Cristalia opposent des équipes de 4 joueurs. Vous pouvez ici constituer une ou plusieurs équipes. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elle agiront en combat, donc ne mettez pas vos meilleures unités en dernier !");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
	
			gameObjectPosition=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getNewsfeedBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getDeckBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 1: // Encart "boutique"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Recruter des unités");
				this.setPopUpDescription("Vous pourrez trouver ici les dernières promotions du centre de recrutement pour renforcer vos équipes. De nouvelles offres apparaissent régulièrement !");
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
		case 2: // Encart "Réseau social"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Mon tableau de bord");
				this.setPopUpDescription("Ce tableau de bord offert à tous les colons fraichement débarqués sur Cristalia permet d'accéder aux actualités de la planète, et de communiquer avec d'autres colons");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getNewsfeedBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getNewsfeedBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 3: // Encart "jouer"
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Combattre");
				this.setPopUpDescription("La meilleure manière de s'enrichir reste de combattre d'autres colons ! Choisissez soigneusement votre combat et vos unités pour affronter d'autres colons et piller leur cristal");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewHomePageController.instance.getPlayBlockOrigin();
			gameObjectPosition2=NewHomePageController.instance.getDeckBlockOrigin();
			gameObjectSize=NewHomePageController.instance.getPlayBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
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
		return NewHomePageController.instance.returnCardFocused ();
	}

	#endregion
}

