using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class SkillBookHelpController : HelpController 
{

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
		}
	}
	public override void getHelpNextAction()
	{
		if (sequenceId < 1) 
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
			StartCoroutine (NewSkillBookController.instance.endHelp ());
			this.quitHelp ();
		}
	}

	#endregion
}

