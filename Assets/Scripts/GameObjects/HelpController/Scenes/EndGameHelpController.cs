using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameHelpController : HelpController 
{
	
	#region help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		default:
			base.getDesktopHelpSequenceSettings();
			break;
		}
	}
	public override void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
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

