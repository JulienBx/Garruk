using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileTutorialController : TutorialObjectController 
{
	public static ProfileTutorialController instance;
	
	public override void endInitialization()
	{
		NewProfileController.instance.endTutorialInitialization ();
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
		case 0: // Encart mon profil ou celui d'un autre joueur
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Mon profil");
					this.setPopUpDescription("Votre vie de colon sur Cristalia vous permettra de rencontrer d'autres colons. Ce profil est votre carte d'identité accessible de tous sur la Cristalosphère.");
				}
				else
				{
					this.setPopUpTitle("Profil d'un colon");
					this.setPopUpDescription("Vous pouvez accéder ici au profil d'un colon, et rentrer en relation avec lui. Ce système de mise en relation vous permettra notamment de vous entrainer face à lui dans les arènes privatisées de Cristalia");
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectSize=NewProfileController.instance.getProfileBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 1: // Encart mes trophées et challenges ou mes confrontations (selon le profil)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Trophées et challenges");
					this.setPopUpDescription("De nombreux trophées sillonneront votre présence sur Cristalia, récompensant votre aptitude au combat, vos connaissances du Cristal ou votre activité sur la planète");
				}
				else
				{
					this.setPopUpTitle("Derniers combats");
					this.setPopUpDescription("Vous pourrez également accéder à la liste des derniers combats du colon, et consulter ainsi l'état de forme de ses équipes");
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewProfileController.instance.getResultsBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectSize=NewProfileController.instance.getResultsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 2: // Encart mes amis ou les amis du joueur (selon le profil)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Mes amis");
					this.setPopUpDescription("Vous créer une vie sociale sur Cristalia vous permettra d'entrainer vos équipes à plusieurs, et de rompre avec la solitude du combattant, mal fréquent chez les colons fraichement débarqués.");
				}
				else
				{
					this.setPopUpTitle("Les amis");
					this.setPopUpDescription("Vous pouvez ici consulter la liste des amis du colon");
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectSize=NewProfileController.instance.getFriendsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			break;
		case 3: // Encart recherche d'amis
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Rechercher un colon");
				this.setPopUpDescription("Recherchez ici un colon en inscrivant sur son nom!");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewProfileController.instance.getSearchBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectSize=NewProfileController.instance.getSearchBlockSize();
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

	#endregion
	
}

