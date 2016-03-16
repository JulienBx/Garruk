using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyHelpController : HelpController 
{
	void Update()
	{

	}
	public void initialize()
	{

	}
	public override void showSequence()
	{
		switch (this.sequenceId) 
		{
		case 0:
			break;
		}
	}
	public override void nextButtonHandler()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.sequenceId++;
			this.showSequence ();
			break;
		}
	}

}

