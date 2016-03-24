using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class MarketHelpController : HelpController 
{

	public override GameObject getFocusedCard()
	{
		return NewMarketController.instance.returnCardFocused();
	}
	public override Vector3 getFocusedCardPosition()
	{
		return NewMarketController.instance.getFocusedCardPosition();
	}

	#region Help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewMarketController.instance.returnCardsBlock (),true);
			this.setCompanion (WordingMarketHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewMarketController.instance.returnFiltersBlock (),true);
			this.setCompanion(WordingMarketHelp.getHelpContent(1),true,true,true,0f);
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
			if (NewMarketController.instance.getAreFiltersDisplayed()) 
			{
				NewMarketController.instance.slideLeft();
			} 
			else if (NewMarketController.instance.getIsMarketContentDisplayed()) 
			{
				NewMarketController.instance.slideRight ();
			}
			NewMarketController.instance.resetScrolling();
			this.setFlashingBlock (NewMarketController.instance.returnCardsBlock (),true);
			this.setCompanion (WordingMarketHelp.getHelpContent (0), true, true, true, 5.5f);
			break;
		case 1:
			this.setFlashingBlock (NewMarketController.instance.returnFiltersBlock (),true);
			NewMarketController.instance.slideRight ();
			this.setCompanion(WordingMarketHelp.getHelpContent(1),true,true,false,0f);
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
		else if(NewMarketController.instance.getIsCardFocusedDisplayed())
		{
			if(NewMarketController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsSkillFocusedDisplayed())
			{
				this.sequenceId=200;
			}
			else if(NewMarketController.instance.returnCardFocused().GetComponent<NewFocusedCardController>().getIsNextLevelPopUpDisplayed())
			{
				this.sequenceId=300;
			}
			else
			{
				this.sequenceId=100;	
			}
			this.launchHelpSequence();
		}
		else if (sequenceId < 1) 
		{
			this.sequenceId++;
			this.launchHelpSequence ();
		} 
		else 
		{
			if(ApplicationDesignRules.isMobileScreen)
			{
				NewMarketController.instance.slideLeft();
			}
			StartCoroutine (NewMarketController.instance.endHelp ());
			this.quitHelp ();
		}
	}

	#endregion
}

