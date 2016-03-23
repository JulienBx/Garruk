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

	public override GameObject getFocusedCard()
	{
		return NewHomePageController.instance.returnCardFocused();
	}
	public override Vector3 getFocusedCardPosition()
	{
		return NewHomePageController.instance.getFocusedCardPosition();
	}

	#region tutorial

	public override void getDesktopTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (1), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (2), true, false, false, 0f);
			this.setBackground (true,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 3:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (3), false, false, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (2).x,MenuController.instance.getButtonPosition (2).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (2).x, MenuController.instance.getButtonPosition (2).y, 3, 1), 1f, 1f);
			break;
		}
	}
	public override void getMobileTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (0), true, false, true, 4f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 1:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (1), true, false, true, 4f);
			this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (2), true, false, false, 4f);
			this.setBackground (true,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 3:
			this.setCompanion (WordingHomePageHelp.getTutorialContent (3), false, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (2).x,MenuController.instance.getButtonPosition (2).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (2).x, MenuController.instance.getButtonPosition (2).y, 1, 1), 0.8f, 0.8f);
			break;
		}
	}
	public override void getTutorialNextAction()
	{
		if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 2) {
			this.sequenceId = 0;
			this.launchTutorialSequence ();
		} else if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 3) {
			this.sequenceId = 1;
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
			this.setFlashingBlock (NewHomePageController.instance.returnDeckBlock (),true);
			this.setCompanion (WordingHomePageHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewHomePageController.instance.returnPlayBlock (),true);
			this.setCompanion(WordingHomePageHelp.getHelpContent(1),true,false,false,0f);
			break;
		case 2:
			this.setCompanion (WordingHomePageHelp.getHelpContent (3), true, true, true, 0f);
			this.setFlashingBlock (NewHomePageController.instance.returnNewsfeedBlock (),true);
			break;
		case 3:
			this.setCompanion(WordingHomePageHelp.getHelpContent(2),true,true,false,0f);
			this.setFlashingBlock (NewHomePageController.instance.returnStoreBlock (),true);
			break;
		default:
			base.getDesktopHelpSequenceSettings();
			break;
		}
	}
	public override void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewHomePageController.instance.returnPlayBlock (),true);
			if (!NewHomePageController.instance.getIsMainContentDisplayed()) 
			{
				NewHomePageController.instance.slideRight ();
			} 
			this.setCompanion (WordingHomePageHelp.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewHomePageController.instance.returnNewsfeedBlock (),true);
			NewHomePageController.instance.slideLeft ();
			this.setCompanion(WordingHomePageHelp.getHelpContent(3),true,true,false,0f);
			break;
		case 2:
			NewHomePageController.instance.slideRight ();
			this.setCompanion (WordingHomePageHelp.getHelpContent (1), true, false, false, 5f);
			this.setFlashingBlock (NewHomePageController.instance.returnDeckBlock (),true);	
			break;
		default:
			base.getMobileHelpSequenceSettings();
			break;
		}
	}
	public override void getHelpNextAction()
	{
		if(this.sequenceId>99)
		{
			base.getHelpNextAction();
		}
		else if(NewHomePageController.instance.getIsCardFocusedDisplayed())
		{
			if(NewHomePageController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsSkillFocusedDisplayed())
			{
				this.sequenceId=200;
			}
			else
			{
				this.sequenceId=100;	
			}
			this.launchHelpSequence();
		}
		else if (sequenceId < 2) 
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

