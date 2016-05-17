using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreHelpController : HelpController 
{

	public override bool getIsScrolling()
	{
		return NewStoreController.instance.getIsScrolling();
	}
	public override GameObject getFocusedCard()
	{
		return NewStoreController.instance.returnCardFocused();
	}
	public override Vector3 getFocusedCardPosition()
	{
		return NewStoreController.instance.getFocusedCardPosition();
	}

	#region tutorial

	public override void getDesktopTutorialSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setCompanion (WordingStoreHelp.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			this.setArrow("left",new Vector3(NewStoreController.instance.returnBuyPackButtonPosition(1).x+1f,NewStoreController.instance.returnBuyPackButtonPosition(1).y,NewStoreController.instance.returnBuyPackButtonPosition(1).z));
			this.setBackground (false,new Rect (NewStoreController.instance.returnBuyPackButtonPosition(1).x, NewStoreController.instance.returnBuyPackButtonPosition(1).y, 3, 1), 1f, 1f);
			break;
		case 2:
			this.setBackground (true,new Rect(0,-0.25f,ApplicationDesignRules.worldWidth+1f,5.5f),0f,0f);
			break;
		case 3:
			this.setCompanion (WordingStoreHelp.getTutorialContent (1), true, false, true, 0f);
			this.setBackground (true,new Rect(0,-0.25f,ApplicationDesignRules.worldWidth+1f,5.5f),0f,0f);
			break;
		case 4:
			this.setCompanion (WordingStoreHelp.getTutorialContent (2), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			break;
		case 5:
			this.setCompanion (WordingStoreHelp.getTutorialContent (3), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().x,-ApplicationDesignRules.upMargin/2f-0.3f-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().y,0f));
			break;
		case 6:
			this.setCompanion (WordingStoreHelp.getTutorialContent (4), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().x,-ApplicationDesignRules.upMargin/2f-0.3f-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().y,0f));
			break;
		case 7:
			this.setCompanion (WordingStoreHelp.getTutorialContent (5), true, false, false, 0f);
			this.setBackground (true,new Rect(+NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("left",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getLifePosition().x+0.15f,-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getLifePosition().y,0f));
			break;
		case 8:
			this.setCompanion (WordingStoreHelp.getTutorialContent (6), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("left",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getSkillsPosition().x+0.3f,-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getSkillsPosition().y,0f));
			break;
		case 9:
			this.setCompanion (WordingStoreHelp.getTutorialContent (7), false, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),1f,1f);
			break;
		case 10:
			this.setCompanion (WordingStoreHelp.getTutorialContent (8), true, false, false, 1.5f);
			this.setBackground (true,new Rect(NewStoreController.instance.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			break;
		case 11:
			this.setCompanion (WordingStoreHelp.getTutorialContent (9), true, false, false, 1.5f);
			this.setBackground (true,new Rect(NewStoreController.instance.getFocusedCardPosition().x,-ApplicationDesignRules.upMargin/2f+NewStoreController.instance.getFocusedCardPosition().y,5.4f,7.7f),0f,0f);
			this.setArrow("down",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getSkillPosition(1).x,0.3f-ApplicationDesignRules.upMargin/2f-ApplicationDesignRules.focusedCardPosition.y+NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getSkillPosition(1).y,0f));
			break;
		case 12:
			this.setCompanion (WordingStoreHelp.getTutorialContent (10), false, false, false, 0f);
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
			this.setCompanion (WordingStoreHelp.getTutorialContent (0), true, false, true, 0f);
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			break;
		case 1:
			if(!this.getCanScroll())
			{
				this.setCanScroll();
			}
			if(NewStoreController.instance.getMediumScrollCameraPosition().y>0.6f)
			{
				this.setScrolling("down",new Vector3(0f,0f,0f));
				this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			}
			else if(NewStoreController.instance.getMediumScrollCameraPosition().y<-2.9f)
			{
				this.setScrolling("up",new Vector3(0f,0f,0f));
				this.setBackground (false,new Rect (0, 10, 5, 5), 0f, 0f);
			}
			else
			{
				this.setArrow("left",new Vector3(NewStoreController.instance.returnBuyPackButtonPosition(1).x+1f,NewStoreController.instance.returnBuyPackButtonPosition(1).y - NewStoreController.instance.getMediumScrollCameraPosition().y + ApplicationDesignRules.bottomBarWorldSize.y/2f,NewStoreController.instance.returnBuyPackButtonPosition(1).z));
				this.setBackground (false,new Rect (NewStoreController.instance.returnBuyPackButtonPosition(1).x, NewStoreController.instance.returnBuyPackButtonPosition(1).y- NewStoreController.instance.getMediumScrollCameraPosition().y + ApplicationDesignRules.bottomBarWorldSize.y/2f, 3, 1), 1f, 1f);
			}
			break;
		case 2:
			this.setBackground (true,new Rect(0,1f,ApplicationDesignRules.worldWidth+1,6f),0f,0f);
			break;
		case 3:
			this.setCompanion (WordingStoreHelp.getTutorialContent (1), true, false, true, 0f);
			this.setBackground (true,new Rect(0,1f,ApplicationDesignRules.worldWidth+1,6f),0f,0f);
			break;
		case 4:
			this.setCompanion (WordingStoreHelp.getTutorialContent (2), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			break;
		case 5:
			this.setCompanion (WordingStoreHelp.getTutorialContent (3), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().x,-ApplicationDesignRules.randomCardsPosition.y-0.3f+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().y,0f));
			break;
		case 6:
			this.setCompanion (WordingStoreHelp.getTutorialContent (4), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,+NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().x,-ApplicationDesignRules.randomCardsPosition.y-0.3f+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getCardTypePosition().y,0f));
			break;
		case 7:
			this.setCompanion (WordingStoreHelp.getTutorialContent (5), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("left",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getLifePosition().x+0.15f,-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getLifePosition().y,0f));
			break;
		case 8:
			this.setCompanion (WordingStoreHelp.getTutorialContent (6), true, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),0f,0f);
			this.setArrow("left",new Vector3(-ApplicationDesignRules.randomCardsPosition.x+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getSkillsPosition().x+0.3f,-ApplicationDesignRules.randomCardsPosition.y+NewStoreController.instance.returnCard(0).GetComponent<NewCardController>().getSkillsPosition().y,0f));
			break;
		case 9:
			this.setCompanion (WordingStoreHelp.getTutorialContent (7), false, false, false, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getCardsPosition(0).x,NewStoreController.instance.getCardsPosition(0).y,NewStoreController.instance.getCardsSize(0).x,NewStoreController.instance.getCardsSize(0).y),1f,1f);
			break;
		case 10:
			this.setCompanion (WordingStoreHelp.getTutorialContent (8), true, true, true, 0f);
			this.setBackground (true,new Rect(NewStoreController.instance.getFocusedCardPosition().x,NewStoreController.instance.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			break;
		case 11:
			this.setCompanion (WordingStoreHelp.getTutorialContent (9), true, false, true, 6f);
			this.setBackground (true,new Rect(NewStoreController.instance.getFocusedCardPosition().x,NewStoreController.instance.getFocusedCardPosition().y,4.2f,5.8f),0f,0f);
			this.setArrow("up",new Vector3(-ApplicationDesignRules.focusedCardPosition.x+NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getSkillPosition(0).x,-0.3f-ApplicationDesignRules.focusedCardPosition.y+NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getSkillPosition(0).y,0f));
			break;
		case 12:
			this.setCompanion (WordingStoreHelp.getTutorialContent (10), false, false, false, 4f);
			this.setArrow ("down", new Vector3(MenuController.instance.getButtonPosition (1).x,MenuController.instance.getButtonPosition (1).y+0.5f,MenuController.instance.getButtonPosition (1).z));
			this.setBackground (false,new Rect (MenuController.instance.getButtonPosition (1).x, MenuController.instance.getButtonPosition (1).y, 1, 1), 0.8f, 0.8f);
			break;
		}
	}
	public override void getTutorialNextAction()
	{
		if (this.sequenceId == -1 && ApplicationModel.player.TutorialStep == 4) {
			this.sequenceId = 0;
			this.launchTutorialSequence ();
		} 
		else if (this.sequenceId < 13) 
		{
			this.sequenceId++;
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
			this.setFlashingBlock (NewStoreController.instance.returnPacksBlock (),true);
			this.setCompanion (WordingStoreHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewStoreController.instance.returnBuyCreditsBlock (),true);
			this.setCompanion(WordingStoreHelp.getHelpContent(1),true,true,true,0f);
			break;
		case 2:
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			this.setCompanion(WordingStoreHelp.getHelpContent(2),true,true,true,0f);
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
			if (!NewStoreController.instance.getIsMainContentDisplayed()) 
			{
				NewStoreController.instance.slideRight ();
			} 
			this.setFlashingBlock (NewStoreController.instance.returnPacksBlock (),true);
			this.setCompanion (WordingStoreHelp.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewStoreController.instance.returnBuyCreditsBlock (),false);
			this.setBackground(true,new Rect(0f,-2.75f,ApplicationDesignRules.worldWidth,2.5f),0f,0f);
			this.setCompanion(WordingStoreHelp.getHelpContent(1),true,true,false,4.5f);
			break;
		case 2:
			this.setBackground (false,new Rect (0, 0, 20, 10), 0f, 0f);
			this.setCompanion(WordingStoreHelp.getHelpContent(2),true,true,true,0f);
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
		else if(NewStoreController.instance.getIsCardFocusedDisplayed())
		{
			if(NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsSkillFocusedDisplayed())
			{
				this.sequenceId=200;
			}
			else if(NewStoreController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsNextLevelPopUpDisplayed())
			{
				this.sequenceId=300;
			}
			else
			{
				this.sequenceId=100;	
			}
			this.launchHelpSequence();
		}
		else if(NewStoreController.instance.getAreRandomCardsDisplayed() && this.sequenceId!=2)
		{
			this.sequenceId=2;
			this.launchHelpSequence();
		}
		else if (sequenceId < 1) 
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

