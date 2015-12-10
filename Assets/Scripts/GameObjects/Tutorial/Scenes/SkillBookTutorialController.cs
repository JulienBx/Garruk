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
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Les compétences");
				this.setPopUpDescription("Le cristal a entrainé des mutations inhabituelles sur les habitants de la planète, conférant à certains d'entre eux des compétences spécifiques. Bien connaitre ses compétences est la clé du succès dans ce monde, et la Cristalopedia vous permettra de vous documenter sur celles-ci.");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			
			gameObjectPosition=NewSkillBookController.instance.getSkillsBlockOrigin();
			gameObjectPosition2=NewSkillBookController.instance.getHelpBlockOrigin();
			gameObjectSize=NewSkillBookController.instance.getSkillsBlockSize();
			this.resizeBackground(new Rect(gameObjectPosition.x,gameObjectPosition.y,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			this.resizePopUp(new Vector3(gameObjectPosition2.x,gameObjectPosition.y,-9.5f));
			break;
		case 1: // Encart les filtres
			if(!isResizing)
			{
				this.displayArrow(false);
				this.displayPopUp(1);
				this.displayNextButton(true);
				this.setPopUpTitle("Rechercher une compétence");
				this.setPopUpDescription("Plus de 150 compétences étant disponibles, des filtres sont à votre disposition pour vous permettre de rechercher des compétences précises");
				this.displaySquareBackground(true);
				this.displayExitButton(true);
				this.displayDragHelp(false);
			}
			gameObjectPosition=NewSkillBookController.instance.getFiltersBlockOrigin();
			gameObjectPosition2=NewSkillBookController.instance.getSkillsBlockOrigin();
			gameObjectSize=NewSkillBookController.instance.getFiltersBlockSize();
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
	
	#endregion
}

