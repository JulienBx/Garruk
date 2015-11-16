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
		case 0: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Mon profil");
					this.setPopUpDescription("A compléter");
				}
				else
				{
					this.setPopUpTitle("Le profil");
					this.setPopUpDescription("A compléter");
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
		case 1: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Mes trophés et challenges");
					this.setPopUpDescription("A compléter");
				}
				else
				{
					this.setPopUpTitle("Mes confrontations");
					this.setPopUpDescription("A compléter");
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
		case 2: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle("Mes amis");
					this.setPopUpDescription("A compléter");
				}
				else
				{
					this.setPopUpTitle("Les amis");
					this.setPopUpDescription("A compléter");
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
		case 3: // Présentation de l'écran de gestion des cartes
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("La recherche");
				this.setPopUpDescription("A compléter");
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
		case 4: // Demande à l'utilisateur de sélectionner des cartes
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}

	#endregion
	
}

