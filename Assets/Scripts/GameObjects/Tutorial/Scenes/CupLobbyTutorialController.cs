using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CupLobbyTutorialController : TutorialObjectController 
{
	public static CupLobbyTutorialController instance;
	
	public override void launchSequence(int sequenceID)
	{
		this.sequenceID = sequenceID;
		switch(this.sequenceID)
		{
		case 0:
			if(!isResizing)
			{
				MenuController.instance.setButtonsGui(false);
				CupLobbyController.instance.setButtonsGui(false);
				view.VM.displayArrow=false;
				view.VM.displayNextButton=true;
				view.VM.title="L'écran de coupe";
				view.VM.description="Cet écran montre votre évolution au sein de la coupe. Vous y verrez votre progression au travers des différents tours ainsi que vos derniers résultats";
			}
			popUpWidth=0.3f*Screen.width;
			popUpHeight=this.computePopUpHeight();
			popUpX=0.35f*Screen.width;
			popUpY=0.35f*Screen.height;
			view.VM.popUpRect= new Rect (popUpX,popUpY,popUpWidth,popUpHeight);
			break;
		case 1:
			StartCoroutine(CupLobbyController.instance.endTutorial());
			break;
		}
	}
	public override void actionIsDone()
	{
	}
}

