using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class LobbyHelpController : HelpController 
{
	public override void getSequenceSettings()
	{
		Vector3 gameObjectPosition = new Vector3 ();
		Vector2 gameObjectSize = new Vector2 ();

		switch (this.sequenceId) 
		{
		case 0:
			gameObjectPosition=NewLobbyController.instance.getMainBlockOrigin();
			gameObjectSize=NewLobbyController.instance.getMainBlockSize();
			this.setCompanion(WordingLobbyTutorial.getHelpContent(1),true);
			this.setBackground(true,new Rect(gameObjectPosition.x,gameObjectPosition.y-ApplicationDesignRules.topBarWorldSize.y+0.2f,gameObjectSize.x-0.03f,gameObjectSize.y-0.03f),0f,0f);
			break;
		}
	}
	public override void companionNextButtonHandler()
	{
		switch (this.sequenceId) 
		{
		case 0:
			this.sequenceId++;
			this.launchSequence();
			break;
		}
	}

}

