using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class ProfileHelpController : HelpController 
{

	#region Help

	public override void getDesktopHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewProfileController.instance.returnProfileBlock (),true);
			this.setCompanion (WordingProfileHelp.getHelpContent (0), true, false, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewProfileController.instance.returnSearchBlock (),true);
			this.setCompanion(WordingProfileHelp.getHelpContent(1),true,false,false,0f);
			break;
		case 2:
			this.setCompanion (WordingProfileHelp.getHelpContent (2), true, true, true, 0f);
			this.setFlashingBlock (NewProfileController.instance.returnFriendsBlock (),true);
			break;
		case 3:
			this.setCompanion(WordingProfileHelp.getHelpContent(3),true,true,false,0f);
			this.setFlashingBlock (NewProfileController.instance.returnResultsBlock (),true);
			break;
		}
	}
	public override void getMobileHelpSequenceSettings()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.setFlashingBlock (NewProfileController.instance.returnProfileBlock (),true);
			if (NewProfileController.instance.getIsFriendsSliderDisplayed ()) 
			{
				NewProfileController.instance.slideRight ();
			} 
			else if (NewProfileController.instance.getIsResultsSliderDisplayed ()) 
			{
				NewProfileController.instance.slideLeft ();
			}
			this.setCompanion (WordingProfileHelp.getHelpContent (0), true, true, true, 0f);
			break;
		case 1:
			this.setFlashingBlock (NewProfileController.instance.returnResultsBlock (),true);
			NewProfileController.instance.slideRight ();
			this.setCompanion(WordingProfileHelp.getHelpContent(3),true,true,false,0f);
			break;
		case 2:
			NewProfileController.instance.slideLeft ();
			this.setCompanion (WordingProfileHelp.getHelpContent (2), true, false, false, 4f);
			this.setFlashingBlock (NewProfileController.instance.returnSearchBlock (),true);	
			break;
		case 3:
			NewProfileController.instance.slideLeft ();
			this.setCompanion(WordingProfileHelp.getHelpContent(1),true,false,false,0f);
			this.setFlashingBlock (NewProfileController.instance.returnFriendsBlock (),true);
			break;
		}
	}
	public override void getHelpNextAction()
	{
		if (sequenceId < 3) 
		{
			this.sequenceId++;
			this.launchHelpSequence ();
		} 
		else 
		{
			NewProfileController.instance.slideRight();
			StartCoroutine (NewProfileController.instance.endHelp ());
			this.quitHelp ();
		}
	}

	#endregion
}

