using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using TMPro;

public class MyGameHelpController : HelpController 
{

	#region tutorial

	public override void getDesktopTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, false, 0f);
			this.setArrow("right",new Vector3(newMyGameController.instance.getNewDeckButtonPosition().x-0.5f,newMyGameController.instance.getNewDeckButtonPosition().y,newMyGameController.instance.getNewDeckButtonPosition().z));
			this.setBackground (false,new Rect (newMyGameController.instance.getNewDeckButtonPosition().x, newMyGameController.instance.getNewDeckButtonPosition().y, 1.5f, 1.5f), 0.75f, 0.75f);
			break;
		case 2:
			break;
		case 3:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 4:
			//this.setMiniCompanion(true,7.5f);
			this.setBackground(true,new Rect(0,-1.5f,20,7),1f,1f);
			this.setDragging("right",new Vector3(0f,0f,0f));
			break;
		case 5:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, true, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 6:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), true, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (5).x,MenuController.instance.getButtonPosition (5).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (5).x, MenuController.instance.getButtonPosition (5).y, 3, 1), 1f, 1f);
			break;
		case 7:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), true, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (4).x,MenuController.instance.getButtonPosition (4).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (4).x, MenuController.instance.getButtonPosition (4).y, 3, 1), 1f, 1f);
			break;
		case 8:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), true, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (3).x,MenuController.instance.getButtonPosition (3).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (3).x, MenuController.instance.getButtonPosition (3).y, 3, 1), 1f, 1f);
			break;
		case 9:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (3), true, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getHelpButtonPosition().x,MenuController.instance.getHelpButtonPosition().y-0.5f,MenuController.instance.getHelpButtonPosition().z));
			this.setBackground (false,new Rect (MenuController.instance.getHelpButtonPosition().x, MenuController.instance.getHelpButtonPosition().y, 1.5f, 1.5f), 1f, 1f);
			break;
		}
	}
	public override void getMobileTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingHomePageTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, false, 0f);
			this.setArrow("right",new Vector3(newMyGameController.instance.getNewDeckButtonPosition().x-0.5f,newMyGameController.instance.getNewDeckButtonPosition().y,newMyGameController.instance.getNewDeckButtonPosition().z));
			this.setBackground (false,new Rect (newMyGameController.instance.getNewDeckButtonPosition().x, newMyGameController.instance.getNewDeckButtonPosition().y, 1f, 1f), 1f, 1f);
			break;
		case 2:
			break;
		case 3:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 4:
			this.setMiniCompanion(true,0);
			this.setDragging("up",new Vector3(0f,0f,0f));
			break;
		case 5:
			this.setCompanion (WordingStoreTutorial.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		}
	}
	public override void getTutorialNextAction()
	{
		if(ApplicationModel.player.HasDeck)
		{
			if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 5) 
			{
				this.sequenceId = 5;
				this.launchTutorialSequence ();
			} 
			else if(this.sequenceId ==4)
			{
				this.sequenceId=5;
				this.launchTutorialSequence();
			}
			else if(this.sequenceId >4)
			{
				this.sequenceId++;
				this.launchTutorialSequence();
			}
		}
		else if(newMyGameController.instance.isADeckCurrentlySelected())
		{
			if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 5) 
			{
				this.sequenceId = 3;
				this.launchTutorialSequence ();
			}
			else if(this.isMiniCompanionClicked)
			{
				this.isMiniCompanionClicked=false;
				this.sequenceId=3;
				this.launchTutorialSequence();
			} 
			else if(this.sequenceId ==2)
			{
				this.sequenceId=3;
				this.launchTutorialSequence();
			}
			else if(this.sequenceId ==3)
			{
				this.sequenceId=4;
				this.launchTutorialSequence();
			}
		}
		else if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 5) 
		{
			this.sequenceId = 0;
			this.launchTutorialSequence ();
		} 
		else if (this.sequenceId == 0) 
		{
			this.sequenceId=1;
			this.launchTutorialSequence();
		}
		else if (this.sequenceId ==1 && newMyGameController.instance.getIsNewDeckPopUpDisplayed())
		{
			this.sequenceId=2;
			this.launchTutorialSequence();
		}
		else if(this.sequenceId ==2 && !newMyGameController.instance.getIsNewDeckPopUpDisplayed())
		{
			this.sequenceId=1;
			this.launchTutorialSequence();
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

