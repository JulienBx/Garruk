using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ProfileTutorialController : TutorialObjectController 
{
	public static ProfileTutorialController instance;

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
		StartCoroutine(NewProfileController.instance.endHelp ());
		if(ApplicationDesignRules.isMobileScreen)
		{
			if(NewProfileController.instance.getIsFriendsSliderDisplayed())
			{
				NewProfileController.instance.slideRight();
			}
			else if(NewProfileController.instance.getIsResultsSliderDisplayed())
			{
				NewProfileController.instance.slideLeft();
			}
		}
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
		case 0: // Encart mon profil ou celui d'un autre joueur
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(0));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(1));
				}
				else
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(2));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(3));
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectSize=NewProfileController.instance.getProfileBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				if(NewProfileController.instance.getIsFriendsSliderDisplayed())
				{
					NewProfileController.instance.slideRight();
				}
				else if(NewProfileController.instance.getIsResultsSliderDisplayed())
				{
					NewProfileController.instance.slideLeft();
				}
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-2.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}

			break;
		case 1: // Encart mes trophées et challenges ou mes confrontations (selon le profil)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(4));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(5));
				}
				else
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(6));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(7));
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewProfileController.instance.getResultsBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectSize=NewProfileController.instance.getResultsBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewProfileController.instance.slideLeft();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3.5f,-9.5f));	
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 2: // Encart recherche d'amis
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle(WordingProfileTutorial.getHelpContent(8));
				this.setPopUpDescription(WordingProfileTutorial.getHelpContent(9));
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewProfileController.instance.getSearchBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectSize=NewProfileController.instance.getSearchBlockSize();

			if(ApplicationDesignRules.isMobileScreen)
			{
				NewProfileController.instance.slideRight();
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.3f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,0.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 3: // Encart mes amis ou les amis du joueur (selon le profil)
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				if(NewProfileController.instance.getIsMyProfile())
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(10));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(11));
				}
				else
				{
					this.setPopUpTitle(WordingProfileTutorial.getHelpContent(12));
					this.setPopUpDescription(WordingProfileTutorial.getHelpContent(13));
				}
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewProfileController.instance.getFriendsBlockOrigin();
			gameObjectPosition2=NewProfileController.instance.getProfileBlockOrigin();
			gameObjectSize=NewProfileController.instance.getFriendsBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewProfileController.instance.slideRight();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
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

