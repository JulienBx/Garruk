using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class SkillBookHelpController : HelpController 
{

    public override Vector3 getFocusedSkillPosition()
    {
        return NewSkillBookController.instance.getFocusedSkillPosition();
    }

    #region Help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewSkillBookController.instance.returnSkillsBlock (),true);
			this.setCompanion (WordingSkillBookHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewSkillBookController.instance.returnFiltersBlock (),true);
			this.setCompanion(WordingSkillBookHelp.getHelpContent(1),true,true,true,0f);
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
			this.setFlashingBlock (NewSkillBookController.instance.returnSkillsBlock (),true);
			if (NewSkillBookController.instance.getAreFilersDisplayed ()) 
			{
				NewSkillBookController.instance.slideLeft ();
			} 
			else if (NewSkillBookController.instance.getHelpDisplayed ()) 
			{
				NewSkillBookController.instance.slideRight ();
			}
			this.setCompanion (WordingSkillBookHelp.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			NewSkillBookController.instance.slideRight ();
			this.setCompanion(WordingSkillBookHelp.getHelpContent(1),true,false,true,0f);
			this.setFlashingBlock (NewSkillBookController.instance.returnFiltersBlock (),true);
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
        else if(NewSkillBookController.instance.getIsFocusedSkillDisplayed())
        {
            this.sequenceId=400;
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
				NewSkillBookController.instance.slideLeft();
			}
			NewSkillBookController.instance.endHelp ();
			this.quitHelp ();
		}
	}

	#endregion
}

