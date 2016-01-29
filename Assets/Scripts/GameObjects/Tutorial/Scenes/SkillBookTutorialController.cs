using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SkillBookTutorialController : TutorialObjectController 
{
	public static SkillBookTutorialController instance;
	
	public override void endInitialization()
	{
		NewSkillBookController.instance.endTutorialInitialization ();
	}
	public override void setGUI(bool value)
	{
		NewSkillBookController.instance.setGUI(value);
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
		StartCoroutine(NewSkillBookController.instance.endHelp ());
		if(ApplicationDesignRules.isMobileScreen && NewSkillBookController.instance.getAreFilersDisplayed())
		{
			NewSkillBookController.instance.slideLeft();
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
		case 0: // Encart les compétences
			if(NewSkillBookController.instance.getIsFocusedSkillDisplayed())
			{
				this.sequenceID=100;
				goto case 100;
			}
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les compétences");
				this.setPopUpDescription("Le cristal a entrainé des mutations inhabituelles sur les habitants de la planète, conférant à certains d'entre eux des compétences spécifiques. Bien connaitre ses compétences est la clé du succès dans ce monde, et la Cristalopedia vous permettra de vous documenter sur celles-ci.");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewSkillBookController.instance.getSkillsBlockOrigin();
			gameObjectPosition2=NewSkillBookController.instance.getHelpBlockOrigin();
			gameObjectSize=NewSkillBookController.instance.getSkillsBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				if(NewSkillBookController.instance.getAreFilersDisplayed())
				{
					NewSkillBookController.instance.slideLeft();
				}
				else if(NewSkillBookController.instance.getHelpDisplayed())
				{
					NewSkillBookController.instance.slideRight();
				}
				else
				{
					NewSkillBookController.instance.resetScrolling();
				}
				this.resizeBackground(new Rect(0f,0f,gameObjectSize.x-0.03f,7.5f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			}
			break;
		case 1: // Encart les filtres
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Rechercher une compétence");
				this.setPopUpDescription("Plus de 150 compétences étant disponibles, des filtres sont à votre disposition pour vous permettre de rechercher des compétences précises");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewSkillBookController.instance.getFiltersBlockOrigin();
			gameObjectPosition2=NewSkillBookController.instance.getSkillsBlockOrigin();
			gameObjectSize=NewSkillBookController.instance.getFiltersBlockSize();
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewSkillBookController.instance.slideRight();
				this.resizeBackground(new Rect(0,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(0f,-3.5f,-9.5f));
			}
			else
			{
				this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
				this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition2.y,-9.5f));
			}
			break;
		case 2: 
			this.endHelp();
			break;
		case 100:
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(0);
				this.displayNextButton(true);
				this.setPopUpTitle("Compétence");
				this.setPopUpDescription("Le détail d'une compétence... bla bla bla bla");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false,false);
			}
			gameObjectPosition=NewSkillBookController.instance.getFocusedSkillPosition();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.focusedSkillPosition.y-System.Convert.ToInt32(!ApplicationDesignRules.isMobileScreen)*ApplicationDesignRules.upMargin/2f,6.5f*ApplicationDesignRules.focusedSkillScale.x,10f*ApplicationDesignRules.focusedSkillScale.x),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition.x,-3.5f,-9.5f));
			break;
		case 101:
			this.endHelp();
			break;
		default:
			base.launchHelpSequence(this.sequenceID);
			break;
		}
	}
	
	#endregion
}

