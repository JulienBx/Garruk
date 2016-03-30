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

	public override GameObject getFocusedCard()
	{
		return newMyGameController.instance.returnCardFocused();
	}
	public override Vector3 getFocusedCardPosition()
	{
		return newMyGameController.instance.getFocusedCardPosition();
	}

	#region tutorial

	public override void getDesktopTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			this.setArrow("right",new Vector3(newMyGameController.instance.getNewDeckButtonPosition().x-0.5f,newMyGameController.instance.getNewDeckButtonPosition().y,newMyGameController.instance.getNewDeckButtonPosition().z));
			this.setBackground (false,new Rect (newMyGameController.instance.getNewDeckButtonPosition().x, newMyGameController.instance.getNewDeckButtonPosition().y, 1.5f, 1.5f), 0.75f, 0.75f);
			break;
		case 2:
			break;
		case 3:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (1), true, false, false, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 4:
			this.setBackground(true,new Rect(0,-1.75f,20,7),1f,1f);
			this.setDragging("right",new Vector3(0f,0f,0f));
			break;
		case 5:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (2), true, true, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 6:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (3), true, true, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (5).x,MenuController.instance.getButtonPosition (5).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (5).x, MenuController.instance.getButtonPosition (5).y, 3, 1), 1f, 1f);
			break;
		case 7:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (4), true, true, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (4).x,MenuController.instance.getButtonPosition (4).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (4).x, MenuController.instance.getButtonPosition (4).y, 3, 1), 1f, 1f);
			break;
		case 8:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (5), true, true, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getButtonPosition (3).x,MenuController.instance.getButtonPosition (3).y-0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (3).x, MenuController.instance.getButtonPosition (3).y, 3, 1), 1f, 1f);
			break;
		case 9:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (6), true, true, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getHelpButtonPosition().x,MenuController.instance.getHelpButtonPosition().y-0.5f,MenuController.instance.getHelpButtonPosition().z));
			this.setBackground (false,new Rect (MenuController.instance.getHelpButtonPosition().x, MenuController.instance.getHelpButtonPosition().y, 1.5f, 1.5f), 1f, 1f);
			break;
		case 10:
			this.setArrow ("down", new Vector3(-ApplicationDesignRules.focusedCardPosition.x+newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getFocusFeaturePosition(4).x,0.5f-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getFocusFeaturePosition(4).y,0f));
			this.setBackground (true,new Rect(0,-1f,ApplicationDesignRules.worldWidth+1f,8f),1f,1f);
			break;
		}
	}
	public override void getMobileTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			this.setArrow("right",new Vector3(newMyGameController.instance.getNewDeckButtonPosition().x-0.5f,newMyGameController.instance.getNewDeckButtonPosition().y,newMyGameController.instance.getNewDeckButtonPosition().z));
			this.setBackground (false,new Rect (newMyGameController.instance.getNewDeckButtonPosition().x, newMyGameController.instance.getNewDeckButtonPosition().y, 1.5f, 1.5f), 0.75f, 0.75f);
			break;
		case 2:
			break;
		case 3:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (1), true, false, false, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 4:
			this.setCanScroll();
			this.setBackground(true,new Rect(0,-0.425f,ApplicationDesignRules.worldWidth+1,7f),1f,1f);
			this.setDragging("up",new Vector3(0f,0f,0f));
			break;
		case 5:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (2), true, true, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 6:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (3), true, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (5).x,MenuController.instance.getButtonPosition (5).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (5).x, MenuController.instance.getButtonPosition (5).y, 1, 1), 0.8f, 0.8f);
			break;
		case 7:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (4), true, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (4).x,MenuController.instance.getButtonPosition (4).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (4).x, MenuController.instance.getButtonPosition (4).y, 1, 1), 0.8f, 0.8f);
			break;
		case 8:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (5), true, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (3).x,MenuController.instance.getButtonPosition (3).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (3).x, MenuController.instance.getButtonPosition (3).y, 1, 1), 0.8f, 0.8f);
			break;
		case 9:
			this.setCompanion (WordingMyGameHelp.getTutorialContent (6), true, true, false, 0f);
			this.setArrow ("up", new Vector3(MenuController.instance.getHelpButtonPosition().x,MenuController.instance.getHelpButtonPosition().y-0.5f,MenuController.instance.getHelpButtonPosition().z));
			this.setBackground (false,new Rect (MenuController.instance.getHelpButtonPosition().x, MenuController.instance.getHelpButtonPosition().y, 1.5f, 1.5f), 1f, 1f);
			break;
		case 10:
			this.setArrow ("down", new Vector3(-ApplicationDesignRules.focusedCardPosition.x+newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getFocusFeaturePosition(4).x,1f-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getFocusFeaturePosition(4).y,0f));
			this.setBackground (true,new Rect(0,0.1f,ApplicationDesignRules.worldWidth+1,7.8f),1f,1f);
			break;
		}
	}
	public override void getTutorialNextAction()
	{
		if(ApplicationModel.player.HasDeck)
		{
			if (this.sequenceId == -1) 
			{
				this.sequenceId = 5;
				this.launchTutorialSequence ();
			}
			else if(this.sequenceId ==9)
			{
				StartCoroutine(ApplicationModel.player.setTutorialStep(-1));
				this.quitTutorial();
			} 
			else if(this.sequenceId >3)
			{
				this.sequenceId++;
				this.launchTutorialSequence();
			}
		}
		else if(newMyGameController.instance.isADeckCurrentlySelected())
		{
			if (this.sequenceId == -1) 
			{
				this.sequenceId = 3;
				this.launchTutorialSequence ();
			}
			else if(this.sequenceId==4 && newMyGameController.instance.getIsFocusedCardDisplayed())
			{
				this.sequenceId=10;
				this.launchTutorialSequence();
			}
			else if(this.sequenceId==10)
			{
				this.sequenceId=4;
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
			this.setFlashingBlock (newMyGameController.instance.returnCardsBlock (),true);
			this.setCompanion (WordingMyGameHelp.getHelpContent (1), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (newMyGameController.instance.returnDeckBlock (),true);
			this.setCompanion(WordingMyGameHelp.getHelpContent(0),true,true,true,0f);
			break;
		case 2:
			this.setCompanion (WordingMyGameHelp.getHelpContent (2), true, true, false, 0f);
			this.setFlashingBlock (newMyGameController.instance.returnFiltersBlock (),true);
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
			if (!newMyGameController.instance.getIsMainContentDisplayed()) 
			{
				newMyGameController.instance.slideLeft ();
			} 
			newMyGameController.instance.resetScrolling();
			this.setFlashingBlock (newMyGameController.instance.returnCardsBlock(),true);
			this.setCompanion (WordingMyGameHelp.getHelpContent (1), true, true, true, 5.5f);
			break;
		case 1:
			this.setFlashingBlock (newMyGameController.instance.returnFiltersBlock (),true);
			newMyGameController.instance.slideRight ();
			this.setCompanion(WordingMyGameHelp.getHelpContent(2),true,true,false,0f);
			break;
		case 2:
			newMyGameController.instance.slideLeft ();
			this.setCompanion (WordingMyGameHelp.getHelpContent (0), true, false, false, 0f);
			this.setFlashingBlock (newMyGameController.instance.returnDeckBlock (),true);	
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
		else if(newMyGameController.instance.getIsCardFocusedDisplayed())
		{
			if(newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsSkillFocusedDisplayed())
			{
				this.sequenceId=200;
			}
			else if(newMyGameController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsNextLevelPopUpDisplayed())
			{
				this.sequenceId=300;
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
		else 
		{
			this.quitHelp ();
		}
	}

	#endregion
}

