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
		else
		{
			this.sequenceId=300;
			this.launchHelpSequence();
		}
	}

	#endregion
}

