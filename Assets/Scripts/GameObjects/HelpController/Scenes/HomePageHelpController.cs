using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using TMPro;

public class HomePageHelpController : HelpController 
{

	#region tutorial

	public override void getDesktopTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (1), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (2), true, false, false, 0f);
			this.setBackground (true,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 3:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), false, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (1).x,MenuController.instance.getButtonPosition (1).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (1).x, MenuController.instance.getButtonPosition (1).y, 3, 1), 1f, 1f);
			break;
		}
	}
	public override void getMobileTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (0), true, false, true, 4f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (1), true, false, true, 4f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (2), true, false, false, 4f);
			this.setBackground (true,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 3:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), false, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (1).x,MenuController.instance.getButtonPosition (1).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (1).x, MenuController.instance.getButtonPosition (1).y, 1, 1), 0.8f, 0.8f);
			break;
		}
	}
	public override void getTutorialNextAction()
	{
		if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 2) {
			this.sequenceId = 0;
			this.launchTutorialSequence ();
		} else if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 3) {
			this.sequenceId = 0;
			this.launchTutorialSequence ();
		} else if (this.sequenceId < 2) {
			this.sequenceId = 2;
			this.launchTutorialSequence ();
		} else if (this.sequenceId == 2) {
			StartCoroutine(ApplicationModel.player.setTutorialStep(4));
			this.sequenceId++;
			this.launchTutorialSequence ();
		} 
	}

	#endregion

	#region help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewHomePageController.instance.returnDeckBlock ());
			this.setCompanion (WordingHomePageTutorial.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewHomePageController.instance.returnPlayBlock ());
			this.setCompanion(WordingHomePageTutorial.getHelpContent(1),true,false,false,0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageTutorial.getHelpContent (3), true, true, true, 0f);
			this.setFlashingBlock (NewHomePageController.instance.returnNewsfeedBlock ());
			break;
		case 3:
			this.setCompanion(WordingHomePageTutorial.getHelpContent(2),true,true,false,0f);
			this.setFlashingBlock (NewHomePageController.instance.returnStoreBlock ());
			break;
		}
	}
	public override void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewHomePageController.instance.returnPlayBlock ());
			if (!NewHomePageController.instance.getIsMainContentDisplayed()) 
			{
				NewHomePageController.instance.slideRight ();
			} 
			this.setCompanion (WordingHomePageTutorial.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewHomePageController.instance.returnNewsfeedBlock ());
			NewHomePageController.instance.slideLeft ();
			this.setCompanion(WordingHomePageTutorial.getHelpContent(3),true,true,false,0f);
			break;
		case 2:
			NewHomePageController.instance.slideRight ();
			this.setCompanion (WordingHomePageTutorial.getHelpContent (1), true, false, false, 4f);
			this.setFlashingBlock (NewHomePageController.instance.returnDeckBlock ());	
			break;
		}
	}
	public override void getHelpNextAction()
	{
		if (sequenceId < 2) 
		{
			this.sequenceId++;
			this.launchHelpSequence ();
		} 
		else if (sequenceId == 2 && !ApplicationDesignRules.isMobileScreen) 
		{
			this.sequenceId++;
			this.launchHelpSequence ();
		}	
		else 
		{
			this.quitHelp ();
		}
	}

	#endregion
}

